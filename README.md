# 🕒 Overtime Report – Sistema de Gestión de Horas Extra

**Overtime Report** es una aplicación web integral diseñada para que empleados y líderes gestionen solicitudes de **horas extra** con cálculos automáticos basados en días festivos, trabajo nocturno, turnos extendidos y más. Este sistema facilita tanto el registro por parte del colaborador como la validación por parte del líder.

---

## 📌 Funcionalidades principales

### Para empleados
- Registrar solicitudes de horas extra indicando sede, horario y tipo de novedad.
- Ver el resumen de todas sus solicitudes (pendientes, aprobadas, rechazadas).
- Filtrar por fecha o estado.

### Para líderes / aprobadores
- Visualizar todas las solicitudes pendientes.
- Revisar detalles de cada novedad.
- Aprobar o rechazar con observaciones.

---

## 🧰 Tecnologías y herramientas utilizadas

- **Backend:** ASP.NET Core Web API (.NET 7)
- **Frontend:** HTML5, CSS3, JavaScript (Vanilla)
- **Base de datos:** SQL Server
- **Autenticación:**
  - **JWT (JSON Web Token)** para el manejo de sesiones seguras
  - **ASP.NET Identity** para la gestión de usuarios y contraseñas
- **Autorización basada en roles:**
  - `TeamMember`: registrar y consultar solicitudes propias
  - `Leader`: consultar y aprobar/rechazar solicitudes de otros usuarios
- **Arquitectura:** Clean Architecture 
- **Patrones:**
  - **CQRS (Command Query Responsibility Segregation)**
  - **MediatR** para comunicación desacoplada entre capas
- **Pruebas unitarias:** xUnit
- **Validaciones:** en frontend usando JavaScript 

---

## 🧪 Funcionalidades clave

- Registro de solicitudes de horas extra (con sede, fechas y horas)
- Filtro por fecha, estado o sede
- Acceso basado en rol con interfaz específica para cada usuario
- Modal con resumen detallado de cada solicitud
- Gestión por parte del líder: aprobar/rechazar con comentarios
- Inicio de sesión protegido con JWT
- Almacenamiento seguro de contraseñas con Identity
- Validación de formularios para evitar datos inconsistentes

---

## 🧱 Estructura del proyecto
OvertimeReport/
├── OvertimeReportBackend/ # API en .NET 7 con lógica de negocio
├── OverTimeReportFrontend/ # Interfaz del usuario (HTML, CSS, JS)
├── assets/ # Imágenes usadas en el README
└── README.md


---

## 📷 Capturas del sistema

### 🔐 Login
![Login](assets/login.png)

### 📄 Interfaz del empleado – Crear solicitud
![Interfaz Empleado](assets/CreateReport.png)

### 📄 Interfaz del empleado – Mis solicitudes
![Interfaz Empleado](assets/userReportInterface.png)

### 📊 Gestión de reportes como líder
![Gestión Líder](assets/leaderManagingReports.png)

### 📝 Revisión detallada de una solicitud
![Solicitud Detallada](assets/LeaderReportRequestManaging.png)

### 📌 Modal resumen de solicitud
![Resumen](assets/reportInfo.png)

---

## ⚙️ Cómo ejecutar el proyecto

### 🖥 Backend (.NET)

1. Ir a `OvertimeReportBackend/`
2. Abrir con Visual Studio 2022 o superior.
3. Configurar la cadena de conexión en `appsettings.json`.
4. Ejecutar el proyecto con `F5` o `dotnet run`.

### 🌐 Frontend

1. Ir a `OverTimeReportFrontend/`
2. Abrir `login.html` en el navegador.

---

## 🧪 Pruebas

- Pruebas de unidad implementadas con **XUnit**
- Validación de lógica para cálculos de horas extra y flujo de aprobación

---

## 🚀 Estado del proyecto

✅ Funcional y completo 
📌 Enfocado a aplicaciones empresariales de recursos humanos y gestión de personal.

---

## 📚 Aprendizajes

Este proyecto fue desarrollado como iniciativa personal para profundizar en:
- Arquitectura limpia en aplicaciones reales
- Pruebas automatizadas con xUnit
- Manejo de roles y seguridad en aplicaciones web modernas
- Integración de frontend con API .NET

---

## 🧑‍💻 Autor

**Sebastián**  
📧 *sebastianvillanv@gmail.com*  
🔗 *https://github.com/Seiked*

---

## 📄 Licencia

Este proyecto es de uso libre para fines educativos y de portafolio.
