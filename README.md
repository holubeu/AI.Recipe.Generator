# AI Recipe Generator

## Application Overview
The application lets users generate recipes from selected ingredients, review and save recipes, and filter them by name or cooking time. Admins can manage ingredient data, including categories, ingredient visibility, and API keys used for recipe generation.

## Run Locally

### Prerequisites
- Install .NET 10 SDK
- Install Node.js (>=18 LTS) and npm

### - Running the Application
1. Start the backend (.NET 10)
	- Open a terminal in ./backend/AiRecipeGenerator/AiRecipeGenerator.API.
    - Run `dotnet restore`.
	- Run: `dotnet run --launch-profile https`.
	- Backend URL: https://localhost:7292.
	- API docs: https://localhost:7292/scalar/v1.

2. Start the frontend (React, Vite, TypeScript)
	- Open a terminal in ./frontend.
	- Run: `npm install` (first time only).
	- Run: `npm run dev`.
	- **UI should be available at:** http://localhost:5173/

#### How to Add an API Key for AI Recipe Generator

##### Prerequisites
- You must obtain an API Key directly from me.
- Contact me via **Microsoft Teams** or by **email** (a.holubeu) to request the API Key.

##### Steps to Add the API Key
1. **Launch the application**.
2. On the **Start Page**, select the role **Admin**.
3. In the Admin layout, open the **Api Keys** menu.
4. Click the **API Key** button.
5. Enter the API Key you received from me.
6. Press **Save** to store the key.

##### Switching Roles
- After saving the API Key, you can change your role:
  1. Click **Change role** in the top‑right corner of the page.
  2. Select **User**.
  3. You will be redirected to the **User layout main page**.


## How I used AI to build the app

### Backend
- I used GitHub Copilot (Cloud Haiku 4.5 ) in Visual Studio.
- I added a '.github' folder with custom instructions for GitHub Copilot to the root of the repository folder.

#### Key Prompts

1. **Prompt** (Backend structure):
Create projects with folders according to the described Solution structure. All projects should be of type Class Library. Configure the references between all projects. Add required project references for AiRecipeGenerator.API project.

**Context**: solution.

**Result**:
At first, the prompt did not work. GitHub Copilot could not find the instructions that were located in the root of the repository.
After I added the folder with instructions to the Solution, the projects and folders inside the projects were created according to the instructions from the file backend.instructions.md.

**What I changed**:
- Disabled nullable reference types in csproj files.

2. **Prompt** (Database creation):
1). In the AiRecipeGenerator.Database project, create an SQL script to build and populate a SQLite database. The database should be stored in a file named 'recipe-generator-db'. For the table fields, use the data types supported by SQLite.
a) Table Recipes:
- Id: integer, primary key, autoincrement;
- Name: text, not null;
- Description: text, nullable;
- DishType: text, nullable;
- CookingTimeFrom: integer, not null;
- CookingTimeTo: integer, not null;
- Steps: text, not null, string array in JSON format;
- CreatedOn: DateTime, not null;
- UpdatedOn: DateTime, not null;
b) Table IngredientСategories:
- Id: integer, primary key, autoincrement;
- Name: text, not null;
- CreatedOn: DateTime, not null;
- UpdatedOn: DateTime, not null;
c) Table Ingredients:
- Id: integer, primary key, autoincrement;
- Name: text, not null;
- CategoryId: integer, not null, Foreign key (IngredientСategories.Id);
- IsVisibleOnCard: boolean, not null;
- CreatedOn: DateTime, not null;
- UpdatedOn: DateTime, not null;
- ImagePath: text, nullable;
d) Table ApiKeys:
- Id: integer, primary key, autoincrement;
- Key: text, not null;
- CreatedOn: DateTime, not null;
2). Create a script to populate the tables.
- Fill the IngredientCategories table with the following ingredient categories (the order must be as specified):'Meat & Poultry', 'Fish & Seafood', 'Eggs', 'Grains & Cereals', 'Legumes & Nuts', 'Vegetables', 'Fruits', 'Dairy', 'Mushrooms'.
- Generate a list of the most popular ingredients (products) for each of the categories listed above. Each ingredient category should contain no more than 10 items. For each category, select the 5 most popular ingredients and set the value of the IsVisibleOnCard column to True when populating the table. Set the ImagePath value to NULL for all entries.
Use the following categories:
Id = 1, 'Meat & Poultry';
Id =2, 'Fish & Seafood';
Id = 3, 'Eggs',
Id = 4, 'Grains & Cereals',
Id = 5, 'Legumes & Nuts',
Id = 6, 'Vegetables',
Id = 7, 'Fruits',
Id = 8, 'Dairy',
Id = 9, 'Mushrooms'.
Populate the Ingredients table with this data.
- Populate the ApiKeys table. Key = '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'.
3). Add the connection string to the database in the appropriate place so that it can later be used to create a connection for Dapper. 
4). Add logic that checks at application startup whether the database file exists and the database is not empty. If the database file does not exist or the database is empty, the database creation script and the table population script should be executed.

**Context**: solution.

**Result**:
Added:
- Scripts for creating and populating the database.
- SQLite NuGet package installed.
- Database initialization service.
- Database existence check.

**What I changed**:
- Added required using directives to Program.cs file.
- Moved IDatabaseInitializationService interface to a separate file.
- Updated instructions in backend.instructions.md.

3. **Prompt** (Repositories):
Create classes and repository interfaces for each table in the AiRecipeGenerator.Database project. Add method implementations for each repository. For all methods returning collections, add pagination support. Create the necessary models for the input parameters of the methods and for the returned results.
- ApiKeyRepository:
    - GetLatestAsync: returns the most recently added Key (only the Key value).
    - AddAsync.
- RecipeRepository
    - GetAsync: accepts as parameters a partial or full name, DishType, and cooking time. Parameters are optional.
    - UpdateAsync.
    - AddAsync.
    - DeleteAsync: deletes by Id.
- IngredientCategoryRepository:
    - GetAllAsync.
    - UpdateAsync.
    - AddAsync.
- IngredientRepository:
    - GetAsync: accepts as parameters a partial or full name, CategoryId. Parameters are optional.
    - UpdateAsync.
    - AddAsync.
    - DeleteAsync: deletes by Id.

**Result**:
- Created models, interfaces, and repository classes.
- Added implementation of the described methods.

**What I changed**:
- Added to backend.instructions.md notes on using primary constructors and simplified using statements (without braces).
- See next prompt: renaming repository method input models to plural form.

4. **Prompt**:
The following instructions apply for all the repositories.  Update the names of the models used as input parameters in repository methods that return either collections or Paginated

**Result**:
- Renamed classes and files.
- Refactoring has been completed as specified.

**What I changed**: -

5. **Prompt** (Tests):
Add a new Class Library project 'AiRecipeGenerator.Database.Tests' to the physical folder Tests. Add this project to the Solution folder Tests. Install required packages. Use xUnit, NSubstitute, AutoFixtere, AutoFixture.Xunit2. Create unit tests for all the repositories. The naming format for test classes is '{Name of the tested class}Tests'. Unit tests must be located in the AiRecipeGenerator.Database.Tests project, inside a folder with the same name as the folder containing the tested class. Repository tests must verify that SQLiteConnection methods are invoked.

**Result**:
- Dummy tests have been created for repositories.
- The test project was not added to the Solution.

**What I changed**:
- Asked GitHub Copilot to create integration tests for repositories.
- Requested moving SQLiteConnection creation into a factory class.

6. **Prompt** (Service layer):
- Create services for each repository in the 'AiRecipeGenerator.Application' project.
- One service per repository.
- Add methods to call each public method of the repositories.
- Create static mappers in the Mappings folder.
- Create the required DTO models in the Dtos folder.
- Create a new static class 'ServiceRegistration' in the root of the project 'AiRecipeGenerator.Application'.
- Use 'Microsoft.Extensions.DependencyInjection' to add the registration of all services in the dependency injection container.
- Add the call to this registration at application startup.

**Result**:
- Created services, interfaces, mappers, and DTOs.
- Service methods accept Query and Command models, which contradicts my backend.instructions.md, but I accepted this approach.
- All mapper methods were public (even helper methods).

**What I changed**:
- Refactored the mappers with GitHub Copilot (see next prompt).

7. **Prompt** (Refactoring):
Refactor the mappers to make the helper methods private. The order of methods in the files must match the order in which they are called. Public methods should be placed at the top of the class and private methods should be placed below the public methods. At the beginning of each mapper method add a null check for input parameters: 'ArgumentNullException.ThrowIfNull(source);'.

**Result**:
- Refactored successfully.

**What I changed**:
- Refactored one of the mappers with GitHub Copilot (changed type added the exception handling).

8. **Prompt** (API Layer):
- In the 'AiRecipeGenerator.API' project in the Controllers folder create controllers. There should be one controller per repository.
- Add the implementation of the controller methods.
- Create the necessary models for requests and responses. Create mappings for requests and responses.

**Result**:
- Created controllers, mappers, request and response models.
- Empty request classes were created for methods without parameters.

**What I changed**:
- Removed unnecessary empty request classes.
- In controller methods where services do not return a result changed the return result to 'NoContent()'.
- Minor edits.

9. **Prompt**:
Add new HttpGet methods 'GetAllAsync' to IngredientsController, IngredientService and IngredientRepository to retrieve all the ingredients grouped by category name. Create new required models and mappings. The controller method should return a response with the following structure:
[
  {
    "category": "string",
    "ingredients": [
      {
        "name": "string",
        "iVisibleOnCard": "boolean",
        "imagePath": "string"
      }
    ]
  }
].

**Result**:
- Created new methods along with new models and mappings.

**What I changed**:
- Fixed parsing for a few fields of repository model.

10. **Prompt** (Service for making API calls to OpenRouter):
Create a new service OpenRouterService in the 'AiRecipeGenerator.Application' project. Add a new method GenerateRecipeAsync. The method must call the OpenRouter API (https://openrouter.ai/api/v1/chat/completions) using the model 'arcee-ai/trinity-large-preview:free'. It should accept a new model GenerateRecipeQueryModel containing a string array of ingredient names. Within the method call IApiKeyService.GetLatestAsync to retrieve the API key which is then used in the API request. The prompt template should be hardcoded in a string variable. The list of ingredients must be converted into a comma separated string and injected into the prompt template.
- The API response has the following structure:
'{
  "choices": [
    {
      "message": {
        "content": ""
      }
    }
  ]
}'.
- The value of the content field is a JSON with the following structure:
'{
  "recipeFound": "boolean",
  "message": "string",
  "recipe": {
    "name": "string",
    "description": "string",
    "dishType": "string",
    "steps": [
      "string"
    ],
    "cookingTime": {
      "from": "number",
      "to": " number "
    }
  }
}'.
- Create models corresponding to the structure of the API response and the content field. Deserialize both the API response and the content field into the existing models.
- Return a GeneratedRecipeDto to the controller. If the content field cannot be deserialized a custom exception must be thrown. Register the method of the OpenRouterService in the DI container.
- Finally add a new method GenerateRecipeAsync to the RecipesController which should call the OpenRouterService method.

**Result**:
- Created a new controller method and a new OpenRouterService.
- Implemented an almost working method for calling the OpenRouter API.
- Response string cleanup before deserialization was not handled.
- Controller method accepts an array of strings instead of a request model.
- Model mapping is done directly inside methods.

**What I changed**:
- Added a method to clean the response string before deserialization.
- Added a request model and mappers.
- Refactoring with the help of GitHub Copilot (see next prompt).

11. **Prompt** (Refactoring):
- Move input parameter of the method GenerateRecipeAsync to a new Request model.
- Rename the mapper GeneratedRecipeResponseMappings to GeneratedRecipeMappings and add a method to map the request model of GenerateRecipeAsync method to GenerateRecipeQueryModel.

**Result**:
- New request model created.
- Mapper renamed and enhanced.

**What I changed**: -

12. **Prompt** (Authentication):
Implement a simplified role‑based authentication. There should be two roles: User and Admin. The role will be passed in the request headers. Create middleware to extract the role from the request header. Create a RoleAuthorizeAttribute to restrict access to endpoints.

**Result**:
- Created RoleMiddleware, RoleAuthorizeAttribute, unit tests.

**What I changed**:
- Assigned appropriate roles for all endpoints.

13. **Prompt** (Error handling):
Add error handling at the API level so that in case an exception is thrown the frontend receives a structured response showing the HttpStatusCode, the error message, and other fields if necessary.

**Result**:
- Created error handling middleware, custom exceptions and unit tests for the middleware class.

**What I changed**:
- There were errors in several tests. I fixed them using GitHub Copilot.

14. **Prompt** (CORS):
Configure CORS so that the API accepts requests from the frontend. Allowed origin: http://localhost:5173. Allow the methods GET, POST, PUT, DELETE and the default headers.

**Result**:
- Configured CORS.

**What I changed**: -

### Frontend
- I used GitHub Copilot (GPT-5.2-Codex) in VS Code.

#### Key Prompts

1. **Prompt** (Structure):
Create a React (Vite + TypeScript) frontend structure for an ASP.NET Core API.
Keep it simple, SPA, no SSR, no overengineering. The frontend must be placed in the 'frontend' folder. Try to act autonomously.

**Result**:
- Created a minimal Vite + React + TypeScript SPA in the frontend folder with a simple Api client (just a structure + config files).

**What I changed**: -

2. **Prompt** (Start page + Router):
Implement the following requirements:
- The application should have two layouts (for User and Admin).
- When the application starts it should always open a start page with a role selection. This should be two buttons (User and Admin).
- When the user clicks one of the buttons they are redirected to the default page for either User or Admin.
- Implement a Router to handle navigation between pages.
- For API requests the request header X-User-Role should include the role selected on the start page. The role can be either 'admin' or 'user'.

**Result**:
- Added Router and role-guarded routes in App.tsx.
- Role storage helper in roleStorage.ts.
- API client includes X-User-Role header in apiClient.ts.
- Start page with User/Admin buttons in StartPage.tsx.
- User/Admin layouts with outlets in UserLayout.tsx and AdminLayout.tsx.
- Default pages in UserHome.tsx and AdminHome.tsx.
- Added router dependency and basic styles in package.json and styles.css.

**What I changed**:
- There was an issue with setting the role in state. I asked to fix that (see next prompt).

3. **Prompt** (Fixinng an issue):
@workspace /fix Once you have chosen the role it's impossible to change it. For example I clicked on User button. Then I redirected to User page. When I click to 'Change role' and then click on Admin button, nothing happens.

**Result**:
Copilot: "Sorry, I can't assist with that."

**What I changed**:
- When I removed '@workspace /fix' and linked the files manually to the context and repeated the prompt, Copilot successfully fixed the issue.

4. **Prompt** (API client):
Update the API client. Add functions to call all backend endpoints. Create the necessary request and response models.

**Context**: Added references to all controllers, request and response models.

**Result**:
- Added models to a single file.
- Added functions for all backend endpoints.

**What I changed**:
- Asked to split the requests and responses into 2 different files.

5. **Prompt** (Navigation menu):
I need a navigation menu visible in both User layout and Admin layout.
Requirements:
- User and Admin layouts have different menu items.
- User layout should have menu items 'Generate Recipe', 'Saved Recipes'.
- Admin layout should have menu items 'Ingredients', 'Ingredient Categories', 'Recipes', 'Api Keys'.
- Menu should be reusable if it makes sense.
- Suggest appropriate component structure
- Implement in React.

**Result**:
- Added reusable component NavigationMenu and wired it into both layouts with role-specific items.
- Created simple placeholder pages and routes so the new menu links land on real screens.

**What I changed**: -

6. **Prompt** (Recipe Generation page):
Create a Recipe Generation page.
Data loading:
- On page load, call API client function 'getAllIngredientsGrouped'
- Display ingredient cards where 'IsVisibleOnCard = true'
- Group ingredient cards by category

Ingredient input:
- Input field for ingredient name
- Add button next to input
- Autocomplete based on all ingredients retrieved from backend
- User may enter ingredient not present in backend list

Ingredient selection:
- Selected ingredients displayed as cards on the right
- Prevent duplicate ingredients by name
- Each selected ingredient card has a small cross button to remove it
- Generate button enabled only if at least one ingredient is selected

Recipe generation:
- Clicking Generate calls API client function 'generateRecipe'
- Display response below selected ingredients area

Recipe response rendering:
- If 'RecipeFound = false':
  - Display 'Message' styled as a warning
- If 'RecipeFound = true':
  - Display each response field on a separate line
  - Display 'Steps' as a bulleted list
  - Display 'CookingTime' as '{From} - {To}'
  - Fields 'Name', 'Description', and 'DishType' are editable
  - Save button calls API client function 'addRecipe'

Card requirements:
- Display image if 'ImagePath' is not empty
- Always display ingredient name
- Selected ingredient cards include remove (cross) button

Additional instruction:
- If any required functionality is unclear or missing, ask for clarification before implementing it.

**Result**:
- Page created successfully.
- All markup is created on one page (no separate components are created).

**What I changed**:
- Found a bug on backend related to dates formatting (fixed).
- Asked to refactor the page and move the ingredient card to a reusable component.

7. **Prompt** (Refactoring):
Refactor the GenerateRecipePage and layout:
- Make the page wider, don't limit the width to 720 pixels.
- Increase the font size on the cards on the left side of the screen.
- Make the cards more noticeable and expressive.
- Rename the field caption Name to Recipe Name.
- Hide the field and its caption if the field value is null or empty.
- Increase the font size of the recipe fields.
- Increase the font size of the editable fields (Recipe Name, Description, Dish Type).
- Increase the spacing between recipe bullet points.
- Rename the caption Steps to Cooking Instructions.
- The hover effect for cards from the Selected ingredients section should be the same as for cards with the ingredient-card class.

**Result**:
- Refactored successfully.

**What I changed**:
- Corrected a format for one label manually.
- Fixed a few css selectors manually.

8. **Prompt** (Saved Recipes page):
Create a Saved Recipes page.  
- Recipes should be displayed as a list, 25 per page.
- The page must support pagination.
- Pagination parameters passed to the backend:
  - Skip = (PageNumber − 1) × PageSize
  - Take = PageSize

- At the top of the page there should be a search field for recipe names and a dropdown list with hardcoded values for maximum cooking time (15, 30, 45, 60, 90, 120, 120, 'doesn't matter'). The dropdown list label: ''Max cooking time, in minutes'. The default value is 'doesn't matter', which means that 'MaxCookingTime = null' is sent to the backend.  
- Each recipe should have Edit and Delete buttons.
- Clicking Edit opens a recipe editing form. Editable fields: Name, DishType, Description.
- Deleting or updating a recipe should be immediately reflected on the page.  

API client functions to be called:  
- Opening the page, clicking pagination buttons, entering a recipe name in the search field, changing the dropdown value: 'getRecipes'.  
- Applying changes in the recipe editing form: 'updateRecipe'.  
- Deleting a recipe: 'deleteRecipe'.

**Result**:
- Page created.
- Recipe instructions are not displayed.
- All markup is created on one page (no separate components were created).

**What I changed**:
- Asked to display recipe instructions and move the edit form to a reusable component.
- Asked to make the GenerateRecipePage the default page for the User layout.

9. **Prompt** (ApiKeys page):
Create ApiKeysPage for Admin layout:
- Display the text: "WARNING: If you add an invalid key, recipe generation will stop working. Here you can add a new API key for the OpenRouter API for the model 'arcee-ai/trinity-large-preview:free'.
- The text should be placed in a separate highlighted section — a dedicated component for displaying warnings and informational blocks to the user.
- Add new 'API key' button, opens the add key form on click.
- On form submit, calls the 'addApiKey' API client function.

**Result**:
- Added a reusable notice component.
- Added the API Keys page.

**What I changed**: -

10. **Prompt** (Ingredients page):
Create IngredientsPage for Admin layout:
- On page load, call API client function 'getIngredients' and 'getIngredientCategories'.
- The page displays a tabular view of ingredients.
- The table shows all ingredient fields except identifiers.
- The page supports pagination.
- No more than 25 rows are displayed per page by default.
- There is an option to select the number of items per page: 25, 50, 100.
- Items can be filtered by category (dropdown list with categories) and by ingredient name.
- Each row in the table has Edit and Delete buttons.
- Above the table there is a button to add a new ingredient.
- Clicking Add or Edit opens a form.
- In the form instead of the CategoryId field a dropdown with category names is displayed.
- The form includes basic validation.
- Create a separate component for the form.
- Adding, editing, and deleting an ingredient should be immediately reflected on the page.
- Deleting an ingredient calls the API client function 'deleteIngredient'.
- Adding an ingredient calls the API client function 'addIngredient'.
- Editing an ingredient calls the API client function 'updateIngredient'.

**Result**:
- Added Ingredients page for Admin layout.
- Added IngredientForm component.

**What I changed**: -

11. **Prompt** (Ingredient Categories page):
Create IngredientCategoriesPage for Admin layout:
- On page load, call API client function 'getIngredientCategories'.
- The page displays a tabular view of ingredient categories.
- The table shows all ingredient categories fields except identifiers.
- The page supports pagination.
- No more than 25 rows are displayed per page by default.
- There is an option to select the number of items per page: 25, 50, 100.
- Each row in the table has Edit button.
- Above the table there is a button to add a new ingredient category.
- Clicking Add or Edit opens a form.
- The form includes basic validation.
- Create a separate component for the form.
- Adding and editing an ingredient category should be immediately reflected on the page.
- Adding an ingredient category calls the API client function 'addIngredientCategory'.
- Editing an ingredient category calls the API client function 'updateIngredientCategory'.

**Result**:
- Added Ingredient Categories page for Admin layout.
- Added IngredientCategoryForm component.

**What I changed**:
- Made Ingredient Categories page the default page for the Admin layout.
- Removed redundant AdminHome page.


## Insights

### What worked well
- Prompts that included **context and constraints** (e.g. "frontend is React + Vite + TypeScript") helped generate code that matched the actual stack.
- **Custom instructions** describing naming conventions, data flow, architecture, required frameworks and libraries, and special requirements.
This eliminates the need to add extra details to each prompt, but it is necessary to ensure that Copilot has read them and added them to the context.
- Adding controllers and API contracts to the context when creating the frontend.
Copilot generates template pages knowing the contracts and the purpose of the application.
- Periodic updates of the instructions in files and notifying Copilot about this, so that it adds the new data to memory.
- Detailed instructions. My prompts contained sufficiently detailed requirements and this worked well. However it is worth to limit the scope of the task (e.g. the Service layer, or conversely the implementation of a single endpoint from controller to repository).
- **Creating a new chat** (session) for tasks not closely related to the previous ones. This seemed to prevent the loss of important instructions from the context.

### What worked poorly
- Using shortcuts like '/fix' without selecting code in the file.
- Working too long in a single chat. Copilot forgot some of my requirements.
- Short (not detailed) instructions for a relatively large task.

### What patterns in prompting gave the best results?
- Structured description of requirements with a request to implement them.
- I found it better to use a few detailed prompts instead of many short ones, since it saved time on checking and fixing.
It's better to take a bit more time to write a detailed prompt than to quickly write short ones and then spend a lot of time fixing the results.