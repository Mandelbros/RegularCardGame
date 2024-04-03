# Regular Card Game

> Proyecto de Programación II. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2022.

`Equipo`: Alex Samuel Bas Beovides (C112) y Ariel González Gómez (C112) 

`Regular Card Game` es un juego de tipo `Trading Cards`, presentando características comunes a la mayoría de juegos pertenecientes a este amplio género:
- Se enfrentan dos `jugadores` en cada `partida`.
- Cada jugador posee un nombre y una imagen que lo identifican, puntos de vida, puntos de mana, y un conjunto de `cartas`, las cuales pueden encontrarse en la `mano`, el `campo` o el `mazo`.
- Cada carta posee un nombre y una imagen que la identifican, un texto de descripción, puntos de vida, puntos de ataque, posibilidad o no de atacar en el turno actual, un coste de mana, y un conjunto de `efectos`.
- Cada efecto tiene una `fase de activación`, unas `condiciones de activación` y unas `acciones`.
- La mano de cada jugador tiene un conjunto de hasta 10 cartas. Cualquier carta que sea añadida a la mano si esta ya está llena es automáticamente destruida.
- El campo de cada jugador tiene un conjunto fijo de 7 casillas en donde se `invocan` las cartas.
- El mazo es el conjunto inicial de cartas de cada jugador.

<br>

## Instrucciones para la ejecución
---
Para jugar tan solo necesitas instalar una versión 6.0.102 o superior de DotNet, y abrir el archivo `Regular Card Game.bat` presente en la carpeta raíz del juego.

<br>

## Breve descripción de una partida
---
Una partida o `match`, se compone de dos jugadores, y una serie de turnos.

Al inicio de la partida ambos jugadores comienzan con 30 puntos de vida, sus correspondientes mazos son reordenados de forma aleatoria, y del mismo sacan 3 cartas. 

Cada jugador, al inicio de su turno, comienza con una cantidad de mana correspondiente al número del turno actual, así en su primer turno tiene 1, en el segundo 2, etc., y en cada turno a partir del décimo tiene 10; asimismo el jugador de turno saca una carta de su mazo (excepto si no le quedan cartas en el mazo, lo cual causa que pierda 1 punto de vida) y se activan automáticamente los efectos de las cartas invocadas de ambos bandos cuya fase de activación sea `comienzo del turno`.

En el turno se pueden realizar en cualquier orden las siguientes acciones:
- Invocar cartas de la mano cuyo costo de mana sea menor o igual al valor de puntos de mana restantes del jugador, de los cuales los primeros se restan. Al invocar una carta se activan sus efectos cuya fase de activación sea `invocación`. Al ser invocadas las cartas no tienen la posibilidad de atacar.
- Atacar con una de las cartas del campo propio a una de las cartas del campo contrario, o directamente al oponente. Al atacar a una carta contraria, se restan de sus puntos de vida los puntos de ataque propios y de los puntos de vida propios los puntos de ataque contrario. Asimismo se activan los efectos de la carta atacante cuya fase de activación sea `atacando`, y los efectos de la carta atacada (si el ataque es contra una carta) cuya fase de activación sea `atacada`. Si cualquiera de ambas cartas es destruida en el encuentro se activan sus efectos cuya fase de activación sea `destruida`. Al atacar directamente se restan de los puntos de vida del oponente los puntos de ataque de la carta. Después de atacar se remueve la posibilidad de atacar en el turno de la carta atacante.
- Finalizar el turno, con lo cual se activan los efectos de las cartas del campo de ambos bandos cuya fase de activación sea `fin del turno`, y comienza el turno del contrario.

La partida termina cuando los puntos de vida de uno de los jugadores se vuelve menor o igual a 0, con lo que su oponente gana.

<br>

## Descripción de la implementación

---

El proyecto fue creado como una aplicación de `BlazorServer`, con lo que está compuesto fundamentalmente por `C#`, `HTML`, `CSS` y `Javascript`.

La aplicación comienza en `Program.cs` que llama a Commands.init() para inicializar los comandos disponibles en el lenguaje de programación creado para el juego, y app.Run() que monta el servidor y levanta la aplicación.

En la carpeta `Cards` se guardan las cartas creadas por los desarrolladores o los jugadores, en formato `JSON`.

En la carpeta `User` se guardan los usuarios creados en formato `JSON`.

En la carpeta `Pages` se encuentran las implementaciones de las distintas escenas del juego:
- Menú principal ![](MainMenu.png)
- Menú de opciones ![](OptionsMenu.png)
- Editor de usuarios ![](UserEditor.png)
- Editor de cartas ![](CardEditor.png)
- Menú de creación de partidas ![](MatchMenu.png)
- Partida ![](Match.png)

En esa misma carpeta se implementa el `layout` aplicado a todas las páginas anteriores, donde se encuentra todo el código Javascript de la solución.

En la carpeta `Shared` se encuentran las implementaciones de componentes de `Blazor` reusables en cualquier parte del proyecto, los cuales son:
- IDE: representa un mini entorno de desarrollo interactivo, en el cual se `programan` los efectos de las cartas. ![](IDE.png)
- EffectDisplay: representa un bloque de código estilizado. ![](EffectDisplay.png)

En la carpeta `Data` se implementa todo el `Back-End` de la aplicación en `C#`, descrito a continuación.

Las características, conceptos, y eventos del juego anteriormente descritos conllevan naturalmente a la implementación de las siguientes clases:
- **Effect**: efecto de una carta.
- **Card**: carta jugable.
- **Deck**: mazo de jugador.
- **User**: usuario del juego.
- **Player**: jugador de la partida.
- **Token**: fragmento clasificable del lenguaje de programación del juego (ej: "while", "(" , "==", "var1").
- **AST**: clase abstracta que representa un nodo de un árbol de sintaxis abstracta (`Abstract Syntax Tree`), y un conjunto de clases que heredan de esta (que representan los distintos tipos de nodo del árbol):
    - **BinOp**: operación binaria
    - **UnaryOp**: operación unaria
    - **Num**: entero con signo
    - **String**: cadena de caracteres
    - **Compound**: bloque de código
    - **Conditional**: bloque if/while
    - **Function**: método/comando del lenguaje
    - **Assign**: sentencia de asignación de variable
    - **Var**: variable
    - **NoOp**: nodo vacío
- **Lexer**: maneja la tokenización del código.
- **Parser**: maneja el análisis sintáctico o gramatical del código y su conversión al árbol de sintaxis abstracta correspondiente, mediante los tokens devueltos por el lexer.
- **Interpreter**: maneja la interpretación del código mediante el recorrido del árbol generado por el parser.
- **Match**: partida del juego.

En los siguientes archivos se dividen e implementan las funcionalidades propias de cada escena:
- **CardEditor.cs**: correspondiente al Editor de Cartas.
- **UserEditor.cs**: correspondiente al Editor de Usuarios.
- **Game.cs**: correspondiente a la Partida. Implementa todo el control de la partida y los jugadores virtuales.

En **Commands.cs** se implementa el manejo del llamado de todas las funciones disponibles del lenguaje del juego. En **Utils.cs** se implementan métodos útiles frecuentemente usados en cualquier parte del proyecto.

En la carpeta `wwwroot` se encuetran las carpetas `audios` (que contiene todos los recursos audibles del juego), `images` (que contiene todas las imágenes del juego) y `css` (que contiene todo el código `CSS` implementado para el proyecto).

<br>

## Lenguaje de programación
---

Para la creación de los efectos de las cartas se creó un mini-lenguaje de programación inspirado en C/C++/C# y Python. Por el momento el lenguaje cuenta con las siguientes características:
- Un programa válido está encapsulado entre llaves "`{ }`"
- Las sentencias se separan por "`;`" (excepto los bloques `if(){}` y `while(){}` ) y no importan los espacios en blanco ni saltos de linea entre tokens.
- Existen variables exclusivamente de tipo entero con signo, y se declaran de la forma "`nombreDeVariable=expresión;`" donde expresión es una expresión matemática que resulta en un valor numérico.
- Las expresiones matemáticas pueden contener números (como `123`, `42`, `0`, etc.), variables (como `i`, `cnt`, `_nombredevariablegenerico2`, etc.), funciones que pueden recibir como argumentos otras expresiones (como `getMyPos()`, `getEnemyLife(i*3+5)`, etc.), operadores binarios (como `+`, `-`, `*`, `/` y `%`) entre dos operandos, operadores unarios (como `+` y `-`) precediendo al operando, y paréntesis agrupando otras expresiones.
- Existen el condicional `if` y el bucle `while`, los cuales reciben una expresión booleana entre paréntesis como condición y un bloque de código entre llaves como cuerpo (ej: `if(cnt>2 && (i<=0 || i>=n)){ i=i+1; }`).
- Las expresiones booleanas son un conjunto evaluaciones booleanas del tipo "`X(EM1 op EM2)`" donde EM1 y EM2 son operaciones matemáticas, op un operador binario booleano (que puede ser `==`, `!=`, `<=`, `>=`, `<`, `>`) y X cualquier número de NOTs lógicos (operador unario `!`), separadas por ANDs u ORs lógicos (operadores `&&` y `||`).
- Una sentencia válida en un bloque de código puede ser una `asignación de variable`, una llamada a una `función/comando`, un bloque `if/while`, o un bloque encapsulado entre llaves `{ ... }`.

Cada carta posee su propia "memoria", es decir, las variables que usan sus efectos son propias de la carta, sin importar que otra carta use una variable con el mismo nombre.

Actualmente las funciones/comandos que se pueden utilizar en el lenguaje son:
- `affectHeroLife(c)`: afecta en c la cantidad de puntos de vida del jugador.
- `affectOpponentLife(c)`: afecta en c la cantidad de puntos de vida del oponente.
- `affectHeroMana(c)`: afecta en c la cantidad de puntos de mana del jugador.
- `affectOpponentMana(c)`: afecta en c la cantidad de puntos de mana del oponente.
- `affectAlliedLife(i,c)`: afecta en c la cantidad de puntos de vida de la carta invocada en la i-ésima casilla del jugador.
- `affectEnemyLife(i,c)`: afecta en c la cantidad de puntos de vida de la carta invocada en la i-ésima casilla del oponente.
- `affectAlliedAttack(i,c)`: afecta en c la cantidad de puntos de ataque de la carta invocada en la i-ésima casilla del jugador.
- `affectEnemyAttack(i,c)`: afecta en c la cantidad de puntos de ataque de la carta invocada en la i-ésima casilla del oponente.
- `getAlliesCount()`: devuelve la cantidad de cartas invocadas en el campo del jugador.
- `getEnemiesCount()`: devuelve la cantidad de cartas invocadas en el campo del oponente.
- `getHeroLife()`: devuelve la cantidad de puntos de vida del jugador.
- `getOpponentLife()`: devuelve la cantidad de puntos de vida del oponente.
- `getTurn()`: devuelve el número del turno actual.
- `random(a,b)`: devuelve un número entero entre a y b incluidos.
- `output(x)`: imprime x.
- `print(s)`: imprime la cadena de caracteres s;
- `break()`: sale del bucle while donde se encuentra;
- `thereIsAllied(i)`: devuelve 1 si existe una carta invocada en la casilla i-ésima del campo del jugador, y 0 en caso contrario.
- `thereIsEnemy(i)`: devuelve 1 si existe una carta invocada en la casilla i-ésima del campo del oponente, y 0 en caso contrario.
- `getAlliedLife(i)`: devuelve la cantidad de puntos de vida de la carta invocada en la casilla i-ésima del campo del jugador, o -1 si esa casilla está vacía.
- `getEnemyLife(i)`: devuelve la cantidad de puntos de vida de la carta invocada en la casilla i-ésima del campo del oponente, o -1 si esa casilla está vacía.
- `getAlliedAttack(i)`: devuelve la cantidad de punos de ataque de la carta invocada en la casilla i-ésima del campo del jugador, o -1 si esa casilla está vacía.
- `getEnemyAttack(i)`: devuelve la cantidad de puntos de ataque de la carta invocada en la casilla i-ésima del campo del oponente, o -1 si esa casilla está vacía.
- `getMyPos()`: devuelve el índice de la casilla en la que está invocada la carta que llamó al efecto actual.

<br>

## Implementación del mini-lenguaje
---

Se dividió el intérprete del lenguaje en 3 componentes fundamentales, en orden de procesamiento:
- `Lexer`: el proceso de descomponer la cadena de caracteres de entrada es llamado Análisis léxico. Por tanto, el primer paso que el intérprete debe realizar es leer la entrada de caractéres y convertirla en una lista ordenada de tokens. Por ejemplo, el programa sencillo siguiente:
    ```cs
    {
        a=2; 
        b=2; 
        c=(a+b)*5;
    }
    ```
    
    Se convierte en la lista de tokens:
    ```cs
    - 'Tipo': {        ,'Valor': {
    - 'Tipo': ID       ,'Valor': a
    - 'Tipo': ASSIGN   ,'Valor': =
    - 'Tipo': INTEGER  ,'Valor': 2
    - 'Tipo': SEMI     ,'Valor': ;
    - 'Tipo': ID       ,'Valor': b
    - 'Tipo': ASSIGN   ,'Valor': =
    - 'Tipo': INTEGER  ,'Valor': 2
    - 'Tipo': SEMI     ,'Valor': ;
    - 'Tipo': ID       ,'Valor': c
    - 'Tipo': ASSIGN   ,'Valor': =
    - 'Tipo': ID       ,'Valor': a
    - 'Tipo': PLUS     ,'Valor': +
    - 'Tipo': ID       ,'Valor': b
    - 'Tipo': MUL      ,'Valor': *
    - 'Tipo': INTEGER  ,'Valor': 5
    - 'Tipo': SEMI     ,'Valor': ;
    - 'Tipo': }        ,'Valor': }
    ```
- `Parser`: el proceso de encontrar la estructura de una lista ordenada de tokens es llamado Análisis sintáctico. El Analizador sintáctico (Parser) se encarga de este proceso, utilizando el Lexer para recibir los tokens en un orden sintácticamente correcto (propio del lenguaje de programación a interpretar), y construyendo lo que se conoce como un Árbol de Sintaxis Abstracta.
    - `AST`: un árbol de sintaxis abstracta, Abstract Syntax Tree (AST) en inglés, es un estructura de datos arbórea que representa la estuctura sintáctica abstracta de un lenguaje donde cada nodo interior y el nodo raíz representa un operador, y los hijos del nodo representan los operandos de tal operador. Por ejemplo, para el código:
    ```cs
    {
        {
            number = 2;
            a = number;
            b = 10 * a + 10 * number / 4;
            c = a - - b
        }
        x = 11;
    }
    ```
- `Interpreter`: una vez contruido el AST correpondiente al programa, lo que queda es correrlo. Esto se hace recorriendo el árbol de cierta manera. De este proceso se encarga nuestra clase `Interpreter.cs`, que recibe el AST devuelto por el parser y "visita" sus nodos. Para cada tipo de nodo del árbol existe un método propio de visita implementado en esta clase, y usualmente resulta en un recorrido tipo Depht First Search (DFS) del subárbol correspondiente al nodo actual. Así una visita a, por ejemplo, un nodo de tipo BinOp (Operación binaria), primero llama a visitar al hijo(operando) izquierdo de la operación y almacenando su valor numérico, lo mismo luego con el hijo(operando) derecho, y dependiendo de la operación en cuestión (`+`\ `-` \ `*` \ `/` \ ...) se devuelve el resultado de la `suma`\ `resta` \ `multiplicación` \ `división` \ ...

Se implementó además un sistema de manejo de excepciones durante la interpretación del código entrado por el usuario, para la correspondiente notificación del error y su posible solución.
    
