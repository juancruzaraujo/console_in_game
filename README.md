# console_in_game
consola in game quake 2 style

consola de estilo quake 2 o counter strike.
en la escena "Console" esta el ejemplo de como se tiene que usar. "Main Camera" tiene el script "ExampleConsole" donde esta la creación de los comandos a usar.
Esta clase es la que uno tiene que modificar en su juego
En el método "Start" se llama al método "CreateCommands" donde se crean los comandos en sí.

La Clase Commands es la encargada de la ejecución, almacenamiento, etc de los comandos.
no se tendría que modificar.

La Clase ManagerConsola, la encargada de manejar la estetica de la consola en sí.
se puede extender a placer de uno.

ver en escala 16:9
la consola se abre con F1, pero eso se puede configurar desde el inspector.

en Unity:
el directorio "Consola" es el que tiene el prefab de la consola, eso es lo que se tendría que agregar
al Hierarchy.
El objecto ManagerConsola es que el contiene a la clase del mismo nombre.
para dar uso, se tiene que crear la clase propia en un objeto común a la escena.
en el ejemplo se pone en la cámara la clase "EscampleConsole" que es la que crea los comandos y demás cosas.

con el tiempo espero ir agregando mas cosas y utilidades.








