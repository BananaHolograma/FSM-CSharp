<p align="center">
	<img width="256px" src="https://github.com/GodotParadise/FSM-Csharp/blob/main/icon.jpg" alt="GodotParadiseFSM logo" />
	<h1 align="center">Godot Paradise FSM</h1>
	
[![LastCommit](https://img.shields.io/github/last-commit/GodotParadise/FSM?cacheSeconds=600)](https://github.com/GodotParadise/FSM-Csharp/commits)
[![Stars](https://img.shields.io/github/stars/godotparadise/FSM)](https://github.com/GodotParadise/FSM-Csharp/stargazers)
[![Total downloads](https://img.shields.io/github/downloads/GodotParadise/FSM-Csharp/total.svg?label=Downloads&logo=github&cacheSeconds=600)](https://github.com/GodotParadise/FSM-Csharp/releases)
[![License](https://img.shields.io/github/license/GodotParadise/FSM?cacheSeconds=2592000)](https://github.com/GodotParadise/FSM-Csharp/blob/main/LICENSE.md)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat&logo=github)](https://github.com/godotparadise/FSM-Csharp/pulls)
[![](https://img.shields.io/discord/1167079890391138406.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/XqS7C34x)
</p>

[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/GodotParadise/FSM-Csharp/blob/main/README.md)

- - -

Una máquina de estados finitos diseñada para cubrir el 95% de los casos de uso, proporcionando funcionalidad esencial y un nodo de estado básico que puede ampliarse.

- [Requerimientos](#requerimientos)
- [✨Instalacion](#instalacion)
	- [Automatica (Recomendada)](#automatica-recomendada)
	- [Manual](#manual)
	- [CSharp GlobalClasses](#csharp-globalclasses)
- [Guia](#guia)
	- [GodotParadiseState](#godotparadisestate)
		- [Enter()](#enter)
		- [Exit()](#exit)
		- [HandleInput(InputEvent @event)](#handleinputinputevent-event)
		- [PhysicsUpdate(double delta)](#physicsupdatedouble-delta)
		- [Update(double delta)](#updatedouble-delta)
		- [OnAnimationPlayerFinished(string name)](#onanimationplayerfinishedstring-name)
		- [OnAnimationFinished()](#onanimationfinished)
	- [Señales](#señales)
- [The Finite State Machine *(FSM)*](#the-finite-state-machine-fsm)
	- [Parámetros exportados](#parámetros-exportados)
	- [Parámetros accessibles como variable](#parámetros-accessibles-como-variable)
	- [Como cambiar de estado](#como-cambiar-de-estado)
	- [Funciones](#funciones)
		- [ChangeState(GodotParadiseState newState, Dictionary parameters, bool force = false)](#changestategodotparadisestate-newstate-dictionary-parameters-bool-force--false)
		- [ChangeStateByName(string name, Dictionary parameters, bool force = false)](#changestatebynamestring-name-dictionary-parameters-bool-force--false)
		- [EnterState(GodotParadiseState state)](#enterstategodotparadisestate-state)
		- [ExitState(GodotParadiseState state)](#exitstategodotparadisestate-state)
		- [GetStateByName(string name)](#getstatebynamestring-name)
		- [bool CurrentStateIs(GodotParadiseState state)](#bool-currentstateisgodotparadisestate-state)
		- [bool CurrentStateNameIs(string name)](#bool-currentstatenameisstring-name)
		- [LockStateMachine()](#lockstatemachine)
		- [UnlockStateMachine()](#unlockstatemachine)
	- [Señales](#señales-1)
- [✌️Eres bienvenido a](#️eres-bienvenido-a)
- [🤝Normas de contribución](#normas-de-contribución)
- [📇Contáctanos](#contáctanos)

# Requerimientos
📢 No ofrecemos soporte para Godot 3+ ya que nos enfocamos en las versiones futuras estables a partir de la versión 4.
* Godot 4+

# ✨Instalacion
## Automatica (Recomendada)
Puedes descargar este plugin desde la [Godot asset library](https://godotengine.org/asset-library/asset/2039) oficial usando la pestaña AssetLib de tu editor Godot. Una vez instalado, estás listo para empezar
## Manual 
Para instalar manualmente el plugin, crea una carpeta **"addons"** en la raíz de tu proyecto Godot y luego descarga el contenido de la carpeta **"addons"** de este repositorio
## CSharp GlobalClasses
Para mostrar en el arbol de escenas y poder añadir tus clases customizadas para los estados de la FSM, necesitas usar [GlobalClasses](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_global_classes.html)

Por ejemplo, si quieres crear una clase `Idle` que hereda de `GodotParadiseState` y hacerla disponible en el arbol de nodos, tienes que usar el decorador:
```csharp
using Godot;
using System;

[GlobalClass, Icon("res://addons/finite_state_machine/state_icon.png")]
public partial class Idle : GodotParadiseState
{
    public override void Enter()
    {
       GD.Print("Idle start");
    }
}

```

La máquina de estados finitos se puede añadir como cualquier otro nodo del árbol de escenas donde se quiera utilizar.

![fsm-add-node](https://github.com/GodotParadise/FSM-Csharp/blob/main/images/fsm_add_child.png)
![fsm-added-scene-tree](https://github.com/GodotParadise/FSM-Csharp/blob/main/images/fsm_added_scene_tree.png)
![fsm-example](https://github.com/GodotParadise/FSM-Csharp/blob/main/images/fsm_example.png)

⚠️ La máquina de estados finitos **siempre necesita al menos un estado por defecto** con el que empezar, este estado por defecto se puede establecer en la variable exportada `CurrentState`. Una vez hecho esto, al ejecutar la escena este será el estado actual de la máquina hasta que se den las condiciones que cambien el estado. Aunque nada se romperá sin él, tener un estado inicial definido es una buena práctica para empezar.

Siempre habrá un solo `_PhysicProcess()` o `_Process_()` ya que es la máquina principal la que se encarga de llamar a los métodos virtuales de cada estado. Si tu estado sobreescribe `PhysicsUpdate()` se ejecutará en el bloque de `_PhysicProcess()`

`Enter()` y `Exit()` son llamadas cuando el nuevo estado se convierte en el actual y cuando se va a transicionar a otro estado. Son útiles para limpiar o preparar algún tipo de parámetros dentro del estado para ser usados sólo en este estado.

# Guia
## GodotParadiseState
Todas las funciones aquí son virtuales, lo que significa que pueden ser sobrescritas con la funcionalidad deseada en cada caso.

En todos los estados tienes acceso a los `PreviousStates` y a los `parameters` extra que has intercambiado entre transición y transición. Los `PreviousStates` sólo están disponibles si has habilitado la pila en el FSM.

```csharp
using Godot;
using Godot.Collections;

public partial class GodotParadiseState : Node
{
	[Signal]
	public delegate void StateEnteredEventHandler();


	[Signal]
	public delegate void StateFinishedEventHandler(string nextState, Dictionary parameters);

	public Array<GodotParadiseState> PreviousStates = new();
	public Dictionary parameters = new();



	public virtual void Enter()
	{

	}

	public virtual void Exit()
	{

	}

	public virtual void HandleInput(InputEvent @event)
	{

	}

	public virtual void PhysicsUpdate(double delta)
	{

	}

	public virtual void Update(double delta)
	{

	}

	public virtual void OnAnimationPlayerFinished(string Name)
	{

	}

	public virtual void OnAnimationFinished()
	{

	}
}

```

### Enter()
Esta función se ejecuta cuando el estado entra por primera vez como estado actual.
### Exit()
Esta función se ejecuta cuando el estado deja de ser el estado actual y pasa al siguiente.
### HandleInput(InputEvent @event)
En caso de que quieras personalizar como este estado maneja las entradas en tu juego este es el lugar para hacerlo. El tipo de evento es InputEvent
### PhysicsUpdate(double delta)
Esta función se ejecuta en cada fotograma del proceso físico de la máquina de estados finitos
### Update(double delta)
Esta función se ejecuta en cada fotograma del proceso de la máquina de estados finitos
### OnAnimationPlayerFinished(string name)
Puedes usar esta función genéricamente para ejecutar lógica personalizada cuando un AnimationPlayer termina cualquier animación. Esta recibe el nombre de la animación como parámetro para evitar errores y ser consistente con la señal original.
### OnAnimationFinished()
Puede usar esta función genéricamente para ejecutar lógica personalizada cuando un AnimatedSprite(2/3)D termina cualquier animación. Esta función no recibe ningún parámetro para evitar errores y ser consistente con la señal original.

## Señales
- *StateEntered*
- *StateFinished(GodotParadiseState next_state, Dictionary parameters)*

Si por ejemplo quieres implementar un estado **Idle** es tan fácil como:
```csharp
using Godot;
using System;

[GlobalClass, Icon("res://addons/finite_state_machine/state_icon.png")]
public partial class Idle : GodotParadiseState {
public override void Enter():
	# play animations...
	# set velocity to zero...

public override void Exit():
	# stop animations...

public override void PhysicsUpdate(double delta):
	# detect the input direction to change to another state such as Walk or Crouch
}

```

# The Finite State Machine *(FSM)*
## Parámetros exportados
- CurrentState: GodotParadiseState = null
- StackCapacity: int = 3
- FlushStackWhenReachCapacity: bool = false
- EnableStack: bool = true
## Parámetros accessibles como variable
- States: Dictionary
- StatesStack: Array[GodotParadiseState]
- Locked: bool

Cuando este nodo está listo en el árbol de escenas, todos los estados detectados como hijos **en cualquier nivel de anidamiento** se guardan en un diccionario para facilitar el acceso por sus nombres de nodo.

La **FSM** se conecta a todas las señales `StateFinished` de los estados anidados existentes. Cuando se produce un cambio de estado y la pila está habilitada, el estado anterior se añade a la pila `StatesStack`. Puedes definir una `StackCapacity` para definir el número de estados anteriores que quieres guardar. Esta pila es accesible en cada estado para manejar condiciones en las que necesitamos saber qué estados han sido transicionados previamente. El valor locked permite bloquear o desbloquear la máquina de estados para la ejecución de estados. Se puede reanudar reseteándolo a false. Cuando está bloqueada **la pila también está deshabilitada.**

## Como cambiar de estado
Este es un ejemplo de código que cambia del estado **Idle** a **Run**:
```csharp
	EmitSignal(SignalName.StateFinished, "Walk", new());
```
Como puedes ver, dentro de cada estado individual, tienes la opción de emitir la señal `StateFinished`, que será monitorizada por la máquina de estados padre.

## Funciones
Normalmente **no se desea llamar a estas funciones manualmente**, es preferible emitir señales desde los propios estados y dejar que la máquina de estados finitos reaccione a estas señales para ejecutar acciones como cambiar el estado. Por cierto, nada te impide hacerlo y puede ser necesario en tu caso de uso.

### ChangeState(GodotParadiseState newState, Dictionary parameters, bool force = false)
Cambia el estado actual al siguiente estado pasado como parámetro si no son el mismo. Esta acción puede forzarse con el tercer parámetro force. Si el estado puede ser transitado, se ejecutará la función `Exit()` del estado actual y la función `Enter()` del siguiente estado. En esta transición el nuevo estado puede recibir parámetros externos. Emite la señal `StateChanged`

### ChangeStateByName(string name, Dictionary parameters, bool force = false)
Realiza la misma acción que la función `ChangeState` pero recibiendo el estado con el nombre que tiene en el diccionario de estados. Por ejemplo, si tenemos un estado de nodo llamado **'Idle'** en la escena, se puede cambiar usando `ChangeStateByName("Idle")`

### EnterState(GodotParadiseState state)
Esta función es llamada cuando un nuevo estado se convierte en el estado actual. Durante este proceso, la señal `StateEntered`  es emitida.


### ExitState(GodotParadiseState state)
Sale del estado actual hacia el proporcionado como parámetro en la función, ejecuta la función `Exit()` antes de salir.
### GetStateByName(string name)
Retorna el nodo de estado usando la clave del diccionario de la variable `States` en la FSM si existe o nulo si no.

### bool CurrentStateIs(GodotParadiseState state)
Comprueba que el estado actual es el proporcionado como parámetro

### bool CurrentStateNameIs(string name)
Mismo que el anterior pero usando la clave del diccionario en formato String.

### LockStateMachine()
Bloquea la FSM, todos los procesos son seteados a false y el stack es deshabilitado. Esta función es llamada automáticamente cuando la variable `Locked` cambia a false.


### UnlockStateMachine()
Desbloquea la máquina si estaba bloqueado, todos los procesos son seteados a true y el stack es habilitado de nuevo. Esta función es llamada automáticamente cuando la variable `Locked` cambia a true.

## Señales
- *StateChanged(GodotParadiseState fromState, GodotParadiseState state)*
- *StackPushed(GodotParadiseState newState, Array<GodotParadiseState> stack)*
- *StackFlushed(Array<GodotParadiseState> stack)*


# ✌️Eres bienvenido a
- [Give feedback](https://github.com/GodotParadise/FSM-Csharp/pulls)
- [Suggest improvements](https://github.com/GodotParadise/FSM-Csharp/issues/new?assignees=BananaHolograma&labels=enhancement&template=feature_request.md&title=)
- [Bug report](https://github.com/GodotParadise/FSM-Csharp/issues/new?assignees=BananaHolograma&labels=bug%2C+task&template=bug_report.md&title=)

GodotParadise esta disponible de forma gratuita.

Si estas agradecido por lo que hacemos, por favor, considera hacer una donación. Desarrollar los plugins y contenidos de GodotParadise requiere una gran cantidad de tiempo y conocimiento, especialmente cuando se trata de Godot. Incluso 1€ es muy apreciado y demuestra que te importa. ¡Muchas Gracias!

- - -
# 🤝Normas de contribución
**¡Gracias por tu interes en GodotParadise!**

Para garantizar un proceso de contribución fluido y colaborativo, revise nuestras [directrices de contribución](https://github.com/godotparadise/FSM-Csharp/blob/main/CONTRIBUTING.md) antes de empezar. Estas directrices describen las normas y expectativas que mantenemos en este proyecto.

**📓Código de conducta:** En este proyecto nos adherimos estrictamente al [Código de conducta de Godot](https://godotengine.org/code-of-conduct/). Como colaborador, es importante respetar y seguir este código para mantener una comunidad positiva e inclusiva.
- - -


# 📇Contáctanos
Si has construido un proyecto, demo, script o algun otro ejemplo usando nuestros plugins haznoslo saber y podemos publicarlo en este repositorio para ayudarnos a mejorar y saber que lo que hacemos es útil.
