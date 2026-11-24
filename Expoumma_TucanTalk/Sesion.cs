namespace Expoumma_TucanTalk
{
    public static class Sesion
    {
        public static string UsuarioActual { get; set; }
        public static bool EstaLogueado => !string.IsNullOrEmpty(UsuarioActual);

        public static void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
