# 📝 HayNotas

Una aplicación de escritorio moderna para Windows que facilita la creación, gestión y compartición de notas en Markdown, potenciada por IA.

## ✨ Características

- **🤖 Asistente IA Integrado**: Crea notas conversando con Google Gemini - simplemente describe lo que necesitas y la IA genera el contenido
- **🎤 Entrada por Voz**: Dicta tus notas usando reconocimiento de voz integrado
- **📄 Formato Markdown**: Todas las notas se guardan en formato Markdown para máxima compatibilidad
- **🌙 Tema Oscuro**: Interfaz elegante con el tema Catppuccin Mocha
- **📤 Compartir Fácilmente**: Envía tus notas por correo electrónico o WhatsApp directamente desde la app
- **💾 Almacenamiento Local**: Tus notas se guardan localmente en tu máquina - tienes control total sobre tus datos

## 🛠️ Tecnologías

- **.NET 8** - Framework moderno de Microsoft
- **WPF** - Windows Presentation Foundation para la interfaz de usuario
- **MVVM Pattern** - Arquitectura Model-View-ViewModel con CommunityToolkit.Mvvm
- **Google Gemini API** - Inteligencia artificial para generación de contenido
- **System.Speech** - Reconocimiento de voz nativo de Windows

## 📋 Requisitos

- Windows 10/11 (64-bit)
- .NET 8 Runtime o superior
- Conexión a Internet (para funcionalidades de IA)

## 🚀 Instalación

### Opción 1: Descargar Release (Recomendado)

1. Descarga la última versión desde [Releases](https://github.com/diego-devs/HayNotas/releases)
2. Extrae el archivo ZIP
3. Ejecuta `HayNotas.exe`

### Opción 2: Compilar desde el código fuente

```bash
# Clonar el repositorio
git clone https://github.com/diego-devs/HayNotas.git
cd HayNotas

# Restaurar dependencias y compilar
dotnet restore
dotnet build

# Ejecutar la aplicación
dotnet run --project src/HayNotas/HayNotas.csproj
```

## ⚙️ Configuración

Al iniciar la aplicación por primera vez, necesitarás configurar:

1. **Google Gemini API Key**:
   - Obtén tu API key gratuita en [Google AI Studio](https://makersuite.google.com/app/apikey)
   - Ingresa la clave en la sección de Configuración de la app

2. **Carpeta de Notas**:
   - Selecciona la carpeta donde deseas guardar tus notas en Markdown
   - Por defecto se usa: `Documentos/HayNotas`

3. **Email (Opcional)**:
   - Configura tu correo SMTP si deseas enviar notas por email
   - Compatible con Gmail, Outlook y otros proveedores

## 📖 Uso

### Crear una Nota con IA

1. Abre la vista de **Chat**
2. Escribe o dicta lo que deseas: "Crea una nota sobre los mejores consejos de productividad"
3. La IA generará el contenido y lo guardará automáticamente como archivo Markdown

### Gestionar Notas

1. Navega a la sección de **Notas**
2. Ve todas tus notas guardadas
3. Edita, elimina o comparte cualquier nota con un clic

### Compartir Notas

- **Email**: Envía la nota como archivo adjunto o en el cuerpo del mensaje
- **WhatsApp**: Comparte a través de WhatsApp Web con un enlace directo

## 📁 Estructura del Proyecto

```
HayNotas/
├── src/
│   └── HayNotas/
│       ├── Models/              # Modelos de datos
│       ├── ViewModels/          # Lógica de presentación (MVVM)
│       ├── Views/               # Interfaces de usuario (XAML)
│       ├── Services/            # Servicios (Chat, Notes, Speech, etc.)
│       ├── Converters/          # Convertidores XAML
│       ├── Helpers/             # Utilidades
│       └── Themes/              # Temas visuales
├── HayNotas.sln                 # Solución de Visual Studio
└── README.md
```

## 🏗️ Arquitectura

La aplicación sigue el patrón **MVVM** (Model-View-ViewModel):

- **Models**: `Note`, `ChatMessage`, `AppSettings`
- **ViewModels**: `MainViewModel`, `ChatViewModel`, `NotesViewModel`, `SettingsViewModel`
- **Services**:
  - `ChatService` - Integración con Gemini API
  - `NotesService` - Gestión de archivos Markdown
  - `SpeechService` - Reconocimiento de voz
  - `SharingService` - Compartir por email/WhatsApp
  - `SettingsService` - Configuración de la app

## 🤝 Contribuir

Las contribuciones son bienvenidas. Para cambios importantes:

1. Fork el repositorio
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Notas de Desarrollo

- La configuración se guarda en: `%AppData%/HayNotas/settings.json`
- Las notas se almacenan en la carpeta configurada por el usuario
- La app usa `WeakReferenceMessenger` para comunicación entre ViewModels

## 📄 Licencia

Este proyecto es de código abierto. Consulta el archivo `LICENSE` para más detalles.

## 👤 Autor

**Diego Sánchez** - [@diego-devs](https://github.com/diego-devs)

---

⭐ Si este proyecto te resulta útil, considera darle una estrella en GitHub
