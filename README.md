1. VISIÓN GENERAL
El proyecto consiste en una solución con dos microservicios independientes:
1.	UserManagementMicroservice
Este microservicio es quien va a administrar la información de los usuarios, como los ítems de trabajo asignados, los cuales pueden ser pendientes o completados, y cuántos son de alta relevancia.
2.	WorkItemMicroservice
Este microservicio es quien va a administrar los ítems de trabajo. Se comunica con el microservicio de usuarios para poder asignar nuevos ítems y actualizar los conteos relevantes.
Cada microservicio utiliza su propia base de datos en memoria con Entity Framework Core. Esto significa que todos los datos se pierden cuando se reinicia la aplicación, pero facilita el desarrollo al no usar una base de datos como tal.
________________________________________
2. ARQUITECTURA Y FLUJO DEL PROYECTO
1.	UserManagementMicroservice
o	Data: UserDbContext donde almacena información de usuarios.
o	Controller: UsersController donde se maneja las solicitud y devuelve las respuestas adecuadas, respecto a las APIs de usuarios.
o	Repositories y Services: Se encarga de la lectura/escritura de usuarios.
o	Datos iniciales: Se inician con algunos usuarios predefinidos para cumplir con las especificaciones dadas para este proyecto, los cuales son UsuarioA y UsuarioB.
2.	WorkItemMicroservice
o	Contexto: WorkItemDbContext donde almacena información de usuarios
o	Controlador: WorkItemsController donde se maneja las solicitud y devuelve las respuestas adecuadas, respecto a las APIs de los ítems.
o	Repositorios y servicios: Se encarga de la creación, actualización y eliminación de ítems de trabajo.
o	Integración con UserManagementMicroservice: Cuando se crea un ítem de trabajo nuevo, se asigna automáticamente a un usuario según especificaciones de relevancia y saturación de este proyecto. Luego actualiza al microservicio de usuarios para cambiar los conteos correspondientes.
________________________________________
3. CONFIGURACIÓN DEL ENTORNO
•	Visual Studio 2022 .NET 8
•	Paquetes NuGet:
o	Microsoft.EntityFrameworkCore
o	Microsoft.EntityFrameworkCore.InMemory
o	Microsoft.AspNetCore.Mvc (incluido por defecto en plantillas Web API).

________________________________________
4. USO DE LAS APIS
A continuación, describiré los endpoints de cada microservicio, qué datos enviar y qué respuesta se obtiene.
•	UserManagementMicroservice

GET https://localhost:puerto/api/users
•	Descripción: Obtiene la lista de todos los usuarios.
•	Ejemplo de Respuesta:
[
  {
    "username": "UsuarioA",
    "highRelevanceCount": 2,
    "pendingItemsCount": 3
  },
  {
    "username": "UsuarioB",
    "highRelevanceCount": 0,
    "pendingItemsCount": 1
  }
]
GET https://localhost:puerto/api/users/{username}
•	Descripción: Consulta un usuario específico de acuerdo a su username.
•	Parámetro: {username} se envía en la ruta, ejemplo: https://localhost:puerto/api/Users/UsuarioA.
•	Ejemplo de Respuesta:
{
  "username": "UsuarioA",
  "highRelevanceCount": 2,
  "pendingItemsCount": 3
}
PUT https://localhost:puerto/api/users/{username}
•	Descripción: Actualiza un usuario existente.
•	Body: Datos del usuario en formato JSON. Debe coincidir el username del body con el de la ruta. Ejemplo de body:
{
  "username": "UsuarioA",
  "highRelevanceCount": 3,
  "pendingItemsCount": 4
}
•	Respuesta: NoContent (204) si la actualización fue exitosa. NotFound (404) si el usuario no existe.
________________________________________
•	WorkItemMicroservice
GET https://localhost:puerto/api/workitems
•	Descripción: Devuelve la lista de todos los ítems de trabajo.
•	Ejemplo de Respuesta:
[
  {
    "id": 1,
    "description": "Revisar documentación",
    "relevance": 1,
    "dueDate": "2024-12-20T00:00:00",
    "assignedUser": "UsuarioA",
    "status": 0
  },
  {
    "id": 2,
    "description": "Desarrollo de módulo A",
    "relevance": 0,
    "dueDate": "2024-12-23T00:00:00",
    "assignedUser": "UsuarioB",
    "status": 0
  }
]
•	relevance: 0 (Low) / 1 (High).
•	status: 0 (Pending) / 1 (Completed).

GET https://localhost:puerto/api/workitems/{id}
•	Descripción: Obtiene un ítem de trabajo, según su id.
•	Respuesta de ejemplo:
{
  "id": 3,
  "description": "Corrección de errores",
  "relevance": “Hight”, 
  "dueDate": "2024-12-18T00:00:00",
  "assignedUser": "UsuarioA",
  "status": 0
}
POST https://localhost:puerto/api/workitems
•	Descripción: Crea un nuevo ítem de trabajo. El microservicio se encarga de asignarlo basándose en las reglas descritas.
•	Body:
En el campo “relevance” puede escribirse el número para relevancia 1 para “High” y 0 para “High” o escribir la palabra “High” y “Low” como tal.

{
  "description": "Nuevo ítem",
  "relevance": 1,
  "dueDate": "2024-12-16T00:00:00",
"assignedUser": "UsuarioB",
}
•	Ejemplo de Respuesta:
{
  "id": 10,
  "description": "Nuevo ítem",
  "relevance": 1,
  "dueDate": "2024-12-16T00:00:00",
  "assignedUser": "UsuarioB",
  "status": “Pending”
}
PUT https://localhost:puerto/api/WorkItems/{id}
•	Descripción: Actualiza un ítem de trabajo existente.
•	Body:
{
  "id": 2,
  "description": "Desarrollo de módulo A - Actualizado",
  "relevance": 1,
  "dueDate": "2024-12-25T00:00:00",
  "assignedUser": "UsuarioB",
  "status": 0
}
•	Respuesta: NoContent (204) si actualiza correctamente. BadRequest (400) si los IDs no coinciden. NotFound (404) si el ítem no existe.
DELETE https://localhost:puerto/api/WorkItems/{id}
•	Descripción: Elimina un ítem de trabajo según su id.
•	Respuesta: NoContent (204) si elimina correctamente; NotFound (404) si no existe.
________________________________________

7. CONSIDERACIONES
•	Base de datos en memoria: Al reiniciar la aplicación, los datos se pierden. Esto es ideal para el desarrollo al no usar una Base de Datos como tal.
•	Comprobaciones de saturación: Un usuario con 3 o más ítems de alta relevancia no recibe nuevos ítems altamente relevantes.
•	Ordenación: Siempre se asignan los ítems al usuario con menor cantidad de pendientes (y menor saturación en alta relevancia).
