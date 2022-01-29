## Projeto - Criando seu e-commerce de livros em C# e Angular

---

#### Apresentaremos uma API de uma pequena Loja Virtual básica em C#, consumida com Angular no Frontend.

> Solução em Asp.Net Net Core 5, onde demonstraremos algumas features interessantes, como: 

### Remodelagem de todo o Backend:

- Re-Arranjo dos Namespaces and global.json created with configuration .Net 5.0 in "src" project folder
- Migração do .Net 3.1 para o .Net 5.0
- Criação dos Projetos Domain, Infra/Data/Repository para separação de responsabilidades por Camadas;
- Product movido para a Domain Layer, dentro da pasta Entities;
- Classe de Contexto da App movida para Infra/Data e namespaces ajustados;
- Install EF Core no Projeto Infra/Data e desinstalado do Projeto API;
- Rename booksstorController para BookstoreController para evitar a Violation Convension;
- Pasta Models renomeada para DTOs, pois nossas models foi movida para o Domínio e trabalharemos com DTOs para transporte de dados;
- Added ComponentModel.Dataannotation in ProductDTO;
- IIS Dev and Self Host config in launchUrl; 
- AddCors Service in Startup;
- Created IGenericRepository in Domain/Abstractions/Base
- GenericRepository (abstract class) created succeed
- IDisposable implemented in IGenericRepository and IRepositoryProducts created;
- IUnitOfWork created in Domain and Injetable IRepositoryProducts, IRepositoryProducts implemented IUnitOfWork
- RepositoryProducts created with IUnitOfWork;
- DI <IRepositoryProducts, RepositoryProducts> in Startup Scoped Life Cicle <= Inversion Of Control;
- Repository and Unit of Work Patterns implemented in PostProduct and Rollback implemented in catch of the Try block;
- Use Repository Pattern in TodoController/GetTodoItems, GetById implemented in TodoController/GetProduct
- Override GetById in RepositoryProducts (id => string) <> GenericRepository;
- Renamed TodoDbContext to ApplicationDbContext;
- Add Attributes ProducesResponseType(typeof(Product), StatusCodes.Status201Created and StatusCodes.Status400BadRequest;
- TodoProduct renamed to Products;
- Add Server=(localdb)\\mssqllocaldb in appsettings global and developer
- Change UseInMemoryDatabase to options.UseSqlServer and AddPolicy (Cors) Development and Production equals;
- SeedData Class with Extension Method Initializer created and Program.cs;
- Install AutoMapper 11.0.0 and AutoMapper<Product, ProductDTO>().ReverseMap() created;
- Registered services.AddAutoMapper(typeof(AutoMapperConfig)) in Startup Class;
- Mapper.Map<>() Product/ProductDTO > Reverse Implemented in ProductController;
- Configured and registered service AddApiConfig() and Install Swagger, SwaggerGen and SwaggerUI v.5.6.3;
- ConfigureSwaggerOptions, Versioning and DefaultValues in Extensions Methods;
- services.AddSwaggerConfig() and app.UseSwaggerConfig(provider) Extension Methods created;
- services.AddTransient<IConfigureOptions<SwaggerGenOptions>, Configured;
- Copy BookstoreController to v1, v2 and v3 and MainController created in Root/Controllers;
- Versioned Controllers inherited from MainController and without ApiController Attributes;
- ApiVersion and Router Changes in Controllers v1, v2 and v3;
- v1 with GetAll only, v2 with GetAll and GetById and v3 with all Methods;
- v1 marked as obsolete (Deprecated);
- launchSettings changes with "launchUrl": "swagger" and Change namespaces of files configurations from Swagger;
- Service and Configure Swagger Extensions segregation;
- Change Route v1, v2 and v3 (Remove empty route);
- ProductMap created in Infra/Mappings;
- ApplyConfiguration and SetColumnType in OnModelCreating;
- Remove and Re-Created Database and Migration;

---

> Este é um resumo dos principais Commits realizados no Upgrade do projeto para a Versão 1.1.0.  No README do Projeto estarão resumidas as funcionalidades implementadas e as alterações e correções de bugs.

> _IMPORTANTE:_ Apenas o Backend foi modificado e testado com o Swagger. 

Torcemos para que gostem!


## Reporting security issues and bugs

Security issues and bugs should be reported privately, via email, to Cooperchip (SAC) at <contato.cooperchip@gmail.com>. You should receive a response within 72 hours. If for some reason you do not, please follow up via email to ensure we received your original message.

## Code of conduct

This project has adopted the [Cooperchip Open Source Code of Conduct](https://cooperchip.com.br/).  For more information, see the [Code of Conduct FAQ](https://cooperchip.com.br/) or contact [contato.cooperchip@gmail.com](mailto:contato.cooperchip@gmail.com) with any additional questions or comments.

> Versão 1 da API com GetById e GetAll, utilizando o DbContext **__acoplado__** na Controller


![Projeto DIO API e-Commerce com Angular e CSharp-v1-obsoleta - Versionado](https://github.com/carlosItDevelop/criando-seu-ecommerce-de-livros-csharp-e-angular/blob/main/imgs/api-v1-obsoleta.png "API Versionada - V1 [Obsoleta]")

> Versão 2 da API com GetById e GetAll com Repository Pattern e UnitOfWork Pattern implementados



![Projeto DIO API e-Commerce com Angular e CSharp-v2 -  Versionado](https://github.com/carlosItDevelop/criando-seu-ecommerce-de-livros-csharp-e-angular/blob/main/imgs/api-v2.png "API Versionada - V2")


> Versão 3 da API com CRUD completo e todos os Patterns implementados


![Projeto DIO API e-Commerce com Angular e CSharp-v3 - Versionado](https://github.com/carlosItDevelop/criando-seu-ecommerce-de-livros-csharp-e-angular/blob/main/imgs/api-v3.png "API Versionada - V3, com CRUD completo")