---
applyTo: 'backend/AiRecipeGenerator/**/*'
---

## Overview
This document defines the backend architecture and coding standards for the AiRecipeGenerator project. All code generation and modifications should follow these guidelines.

## Solution Structure
Here are the projects and the folders they contain. Only the main folders are listed.
- AiRecipeGenerator.Database: Repositories, Data Access.
	- Models.
		- Queries.
		- Commands.
		- Repository: Folder for the models that the data retrieved from an SQL query is mapped to.
- AiRecipeGenerator.Application: Business Logic, Services.
	- Dtos
	- Interfaces.
	- Services.
	- Mappings: Folder for static mappers. Single mapper per entity (For example RecipeMappings). Mappers convert DTO models to query and command models, and vice versa.
	- Exceptions: Folder for custom Exceptions.
	- Constants: Folder for constants.
- AiRecipeGenerator.API: Entry Point, Startup.
	- Controllers: folder for controllers.
	- Mappings.
		- Requests: Folder for static request mappers. Single mapper per request model. Mappers convert request models to DTO models.
		- Responses: Folder for static response mappers. Single mapper per response model. Mappers convert DTO models to request models.
	- Models.
		- Requests.
		- Responses.
	- Middleware.

## Dependency between projects
- AiRecipeGenerator.Database has no dependencies.
- AiRecipeGenerator.Application references AiRecipeGenerator.Database.
- AiRecipeGenerator.API references AiRecipeGenerator.Application and AiRecipeGenerator.Database.

## Technologies and Libraries
- Database: SQLite.
- ORM/Data Access: Dapper.
- Web Framework: ASP.NET Core.
- HTTP Client: HttpClient (built-in .NET library).

## Naming
- Folder, class, method (function), and variable names should be clear and descriptive.
- Models used as input parameters in controller actions must end with the suffix 'RequestModel'.
- Models returned from controller actions must end with the suffix 'ResponseModel'.
- Request and response model names should be based on the action name. For example, if the controller action is GetRecipeCategoriesAsync, then the request and response models should be named GetRecipeCategoriesRequestModel and GetRecipeCategoryResponseModel. Since the response is a collection of objects, the entity name in the response model should be singular (GetRecipeCategoryResponseModel).
- DTO models must use the suffix 'Dto', for example GetRecipeCategoriesDto.
- Names of DTO models sent to the service should be based on the request name. For example, if the request model is GetRecipeCategoriesRequestModel, it should be mapped to GetRecipeCategoriesDto.
- Names of DTO models returned from the service should be based on the RepositoryModel name, for example RecipeCategoryDto.
- Models used as input parameters in repository methods for retrieving data from the database must end with the suffix 'QueryModel'.
- Models used as input parameters in repository methods for writing data to the database must end with the suffix 'CommandModel'. 
- Models returned from repository methods must end with the suffix 'RepositoryModel'.
- The name of a mapper class should be based on the entity name. For example, to handle requests like GetRecipeCategoriesRequestModel and AddRecipeCategoryRequestModel, a single mapper should be created with the name RecipeCategoryRequestMappings.  
- Mapper method names should follow the pattern 'To{Target model name}', for example ToGetRecipeCategoriesDto. If the mapper method returns a collection, the name should use the plural form, such as ToRecipeCategoryDtos or ToRecipeCategoryResponseModels.

## Data flow
- A request model is mapped to a DTO in the controller before being passed to the service method.
- A DTO model is mapped to a query model or command model in the service method before being passed to the repository method.
- A repository model is mapped to a DTO in the service method before being passed to the controller action.

## API Controllers
- Follow REST principles: one controller per entity.
- Use clear and consistent routes with `[Route("api/[controller]")]`.
- Each controller should expose CRUD endpoints for its entity when applicable.
- Prefer plural nouns in routes (e.g. `/ingredients`, `/recipes`).

## Coding Standards
- Every class or interface should reside in its own file.
- Place interfaces in the Interfaces directory and services in the Services directory.
- Prefer using primary constructors.
- Use simplified using statements without braces.