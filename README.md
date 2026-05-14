# Rendimiento Voleibol

Aplicacion de escritorio en Windows Forms para registrar, consultar y analizar el rendimiento deportivo de jugadores de voleibol.

## Funcionalidades principales

- Dashboard principal con resumen del sistema.
- Gestion de jugadores.
- Registro de sesiones de rendimiento.
- Busqueda y analisis de datos.
- Reportes por jugador.
- Simulacion de datos IoT.

## Requerimientos de diseno y usabilidad

- Navegacion: menu lateral para moverse entre Dashboard, Jugadores, Registro, Busqueda, Reportes y Acerca del sistema.
- Acciones basicas: botones para guardar, limpiar, buscar, ver todos, eliminar, generar reportes y simular datos.
- Validacion: controles para evitar campos obligatorios vacios y validar datos como correo, edad y valores numericos.
- Consistencia visual: paleta de colores y tipografias centralizadas en `AppTheme`.
- Adaptabilidad: ventana maximizada, tamano minimo definido, paneles con `Dock`, `Anchor` y desplazamiento donde corresponde.

## Tecnologias

- C#
- .NET 8
- Windows Forms

## Ejecucion

Abrir `RendimientoVoleibol.sln` en Visual Studio y ejecutar el proyecto con `F5`.
