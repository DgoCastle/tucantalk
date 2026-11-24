using System;
using MySql.Data.MySqlClient;

namespace Expoumma_TucanTalk
{
    public static class UserProgress
    {
        // Evento para notificar cambios en el progreso
        public static event Action OnXPChanged;

        // Variables de progreso del usuario
        public static int XP { get; private set; } = 0;
        public static int Nivel { get; private set; } = 1;
        public static int Racha { get; private set; } = 0;
        public static DateTime UltimoDiaJugado { get; private set; } = DateTime.MinValue;

        // Constantes ajustables
        private const int XP_POR_NIVEL = 100; // XP necesaria para subir de nivel

        // Recompensas por tipo de actividad
        private const int XP_GRAMMAR = 50;
        private const int XP_LISTENING = 40;
        private const int XP_READING = 45;
        private const int XP_VOCABULARIO = 30;
        private const int XP_SPEAKING = 60;

        /// <summary>
        /// Registra que el usuario completó un tema o actividad.
        /// </summary>
        public static void TemaCompletado(string tipoActividad)
        {
            int xpGanada;

            switch (tipoActividad)
            {
                case "Grammar":
                    xpGanada = XP_GRAMMAR;
                    break;
                case "Listening":
                    xpGanada = XP_LISTENING;
                    break;
                case "Reading":
                    xpGanada = XP_READING;
                    break;
                case "Vocabulario":
                    xpGanada = XP_VOCABULARIO;
                    break;
                case "Speaking":
                    xpGanada = XP_SPEAKING;
                    break;
                default:
                    xpGanada = 20;
                    break;
            }

            AgregarXP(xpGanada);
            ActualizarRacha();
            Guardar(); // guarda en BD si hay sesión, si no guarda en Settings
        }

        /// <summary>
        /// Suma experiencia directamente.
        /// </summary>
        public static void AgregarXP(int cantidad)
        {
            XP += cantidad;

            if (XP >= Nivel * XP_POR_NIVEL)
                Nivel++;

            Guardar();

            // Dispara el evento para que los formularios actualicen su interfaz
            OnXPChanged?.Invoke();
        }

        /// <summary>
        /// Actualiza la racha de días jugados.
        /// </summary>
        private static void ActualizarRacha()
        {
            if (UltimoDiaJugado.Date == DateTime.Today.AddDays(-1))
                Racha++;
            else if (UltimoDiaJugado.Date < DateTime.Today.AddDays(-1))
                Racha = 1;

            UltimoDiaJugado = DateTime.Today;
        }

        public static void Reiniciar()
        {
            XP = 0;
            Nivel = 1;
            Racha = 0;
            UltimoDiaJugado = DateTime.MinValue;
            Guardar();
            OnXPChanged?.Invoke(); // También notifica al reiniciar
        }

        /// <summary>
        /// Guarda datos: si hay usuario logueado (Sesion.UsuarioActual) guarda en la BD,
        /// si no, guarda en Properties.Settings (modo offline/desarrollo).
        /// </summary>
        public static void Guardar()
        {
            if (!string.IsNullOrEmpty(Sesion.UsuarioActual))
            {
                GuardarEnBD(Sesion.UsuarioActual);
            }
            else
            {
                GuardarLocal();
            }
        }

        /// <summary>
        /// Carga datos: si hay usuario logueado carga desde BD, si no desde Settings.
        /// </summary>
        public static void Cargar()
        {
            if (!string.IsNullOrEmpty(Sesion.UsuarioActual))
            {
                CargarDesdeBD(Sesion.UsuarioActual);
            }
            else
            {
                CargarLocal();
            }
        }

        #region Local (Properties.Settings) - fallback
        private static void GuardarLocal()
        {
            Properties.Settings.Default.XP = XP;
            Properties.Settings.Default.Nivel = Nivel;
            Properties.Settings.Default.Racha = Racha;
            Properties.Settings.Default.UltimoDiaJugado = UltimoDiaJugado;
            Properties.Settings.Default.Save();
        }

        private static void CargarLocal()
        {
            XP = Properties.Settings.Default.XP;
            Nivel = Properties.Settings.Default.Nivel;
            Racha = Properties.Settings.Default.Racha;
            UltimoDiaJugado = Properties.Settings.Default.UltimoDiaJugado;
            OnXPChanged?.Invoke();
        }
        #endregion

        #region MySQL persistence
        private static void GuardarEnBD(string usuario)
        {
            try
            {
                using (var conn = DB.GetConnection())
                {
                    if (conn == null) return;
                    string query = "UPDATE usuarios SET xp = @xp, nivel = @nivel, racha = @racha, ultimoDiaJugado = @ultimo WHERE usuario = @usuario";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@xp", XP);
                        cmd.Parameters.AddWithValue("@nivel", Nivel);
                        cmd.Parameters.AddWithValue("@racha", Racha);

                        if (UltimoDiaJugado == DateTime.MinValue)
                            cmd.Parameters.AddWithValue("@ultimo", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@ultimo", UltimoDiaJugado.Date);

                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        int rows = cmd.ExecuteNonQuery();
                        // Si no existe el usuario por alguna razón, no hacemos nada aquí.
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error guardando progreso en la BD: " + ex.Message);
            }
        }

        private static void CargarDesdeBD(string usuario)
        {
            try
            {
                using (var conn = DB.GetConnection())
                {
                    if (conn == null) return;
                    string query = "SELECT xp, nivel, racha, ultimoDiaJugado FROM usuarios WHERE usuario = @usuario LIMIT 1";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                XP = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader.GetValue(0));
                                Nivel = reader.IsDBNull(1) ? 1 : Convert.ToInt32(reader.GetValue(1));
                                Racha = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetValue(2));

                                if (reader.IsDBNull(3))
                                    UltimoDiaJugado = DateTime.MinValue;
                                else
                                {
                                    // MySQL DATE -> DateTime
                                    var obj = reader.GetValue(3);
                                    UltimoDiaJugado = Convert.ToDateTime(obj);
                                }
                            }
                            else
                            {
                                // Si no se encuentra el usuario (raro), establecer valores por defecto
                                XP = 0;
                                Nivel = 1;
                                Racha = 0;
                                UltimoDiaJugado = DateTime.MinValue;
                            }
                        }
                    }
                }

                OnXPChanged?.Invoke();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error cargando progreso desde la BD: " + ex.Message);
            }
        }
        #endregion

        // Métodos útiles para mostrar progreso relativo
        public static int XPParaNivelActual => Nivel * XP_POR_NIVEL;
        public static int XPRelativo => XP % XP_POR_NIVEL;
        public static double PorcentajeXP => (double)XPRelativo / XP_POR_NIVEL * 100;
    }
}
