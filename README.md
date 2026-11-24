# 🦜 TucanTalk

**App de escritorio para aprender inglés**, al estilo Duolingo, hecha en **C# (.NET Framework, WinForms)** con base de datos **MySQL**. Proyecto en equipo presentado en la **EXPO UMMA 2025**.

## ✨ Funciones
- 🔐 **Registro e inicio de sesión** con contraseñas cifradas (**SHA-256**) y consultas parametrizadas (protección contra inyección SQL).
- 🗣️ **Vocabulary:** palabras con **audio de pronunciación** (profesiones: actor, chef, doctor, engineer...).
- 🎧 **Listening:** ejercicios de comprensión auditiva.
- 📖 **Reading:** comprensión de lectura.
- ✏️ **Grammar:** práctica de gramática.
- 🏆 **Progreso gamificado:** XP, niveles y **racha** de días jugados, guardados en la base de datos.
- 👤 **Cuenta de usuario** con foto de perfil.
- 🎨 Interfaz moderna con **Guna UI2** y navegación por *user controls*.

## 🗂️ Estructura
```
Expoumma_TucanTalk/
├── Controls/       → pantallas: login, registro, jugar, vocabulary, listening, reading, grammar, progreso, cuenta, créditos
├── Forms/          → ventana principal
├── resources/      → audios (pronunciación y listening)
├── DB.cs           → conexión a MySQL
└── Program.cs
database/
└── expoumma_db.sql → estructura de la base de datos (tabla usuarios: xp, nivel, racha, foto, último día jugado)
```

## 🚀 Cómo correrlo
1. Instala **XAMPP** (MySQL/MariaDB) e importa `database/expoumma_db.sql` desde phpMyAdmin.
2. Abre `Expoumma_TucanTalk.sln` en **Visual Studio**.
3. Restaura los paquetes NuGet (clic derecho en la solución → *Restore NuGet Packages*).
4. Si tu MySQL tiene contraseña, ajústala en `DB.cs`.
5. Ejecuta con **F5**.

## 🛠️ Tecnologías
C# · .NET Framework · Windows Forms · MySQL / MariaDB (MySql.Data) · Guna.UI2.WinForms

## 👥 Equipo
Proyecto en equipo para la EXPO de la Universidad Mundo Maya (noviembre 2025). Desarrollo: **Diego Castillo** ([@DgoCastle](https://github.com/DgoCastle)).
