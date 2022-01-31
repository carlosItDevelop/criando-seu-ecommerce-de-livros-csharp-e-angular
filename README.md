## Projeto - Criando seu e-commerce de livros em C# e Angular

---

### Apresentaremos uma API de uma Loja Virtual básica em C#, consumida com Angular.

> Solução em Asp.Net Net Core 5, onde demonstraremos algumas features interessantes, como: 

### Remodelagem de todo o Backend:

> O projeto Backend foi todo remodelado, pois havia algumas falhas de design, que eu acho que foram deixadas de propósito, para que fizéssemos a refatoração.

> Alguns pontos críticos foram identificados e corrigidos em seguida. A saber:

- A primeira alteração significativa foi a **__Migração do Projeto do .Net 3.1 para o .Net 5.0__**. Com isso gravamos um arquivo de controle de SDK na pasta da Solution - global.json. Segue o código fonte:

```json
    {
      "projects": [ "src", "tests" ],
      "sdk": {
        "version": "5.0.100"
      }
    }
```
- __"src" e "tests" são as pastas para os projetos de API, Class Library e Testes.__

- Separei as responsabilidades nas camadas UI/API, Domain e Infra/Data/Repository, com isso movemos o nosso ApplicationDbContext para a camada de Dados e a Model Product para Domain/Entities. Com esse movimento podemos remover o Entityframework Core da API Layer e instalá-lo na camada de Infra/Data.

- Na nossa API layer criamos a DTO para transporte de dados, deixando nosso modelo __Product__ Clean na Domain layer, pois trabalharemos com __Domínio Rico__ e essa abordagem nos proporciona um bom design, enquanto tratamos das validações de modelos na nossa DTO.

- Configurei o __CORS__ em services e configure na Startup para que o Frontend não tenha restrições de políticas de acesso.

- Nossa __API__ foi __versionada__ e adicionamos todas as configurações de __Versioning do Asp.Net Core__ e a documentação do __Swagger__.

- Antes de continuar vamos dar uma olhada no print da __V1 da nossa API__, que está __Obsoleta__:

> ### Versão 1 da API com GetById e GetAll, utilizando o DbContext **__acoplado__** na Controller


![Projeto DIO API e-Commerce com Angular e CSharp-v1-obsoleta - Versionado](https://github.com/carlosItDevelop/criando-seu-ecommerce-de-livros-csharp-e-angular/blob/main/imgs/api-v1-obsoleta.png "API Versionada - V1 [Obsoleta]")

- Abaixo os Packages necessários para configuração do versionamento e documentação do swagger:

```xml
    <Project Sdk="Microsoft.NET.Sdk.Web">

      <PropertyGroup><TargetFramework>net5.0</TargetFramework></PropertyGroup>
      <ItemGroup>
        <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="11.0.0" />

        <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning" Version="5.0.0" />
        <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer" Version="5.0.0" />   
        <PackageReference Include="Swashbuckle.AspNetCore.Swagger" Version="5.6.3" />
        <PackageReference Include="Swashbuckle.AspNetCore.SwaggerGen" Version="5.6.3" />
        <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="5.6.3" />
      </ItemGroup>

      <ItemGroup>
        <ProjectReference Include="..\Bookstore.Domain\Bookstore.Domain.csproj" />
        <ProjectReference Include="..\Bookstore.Infra\Bookstore.Infra.csproj" />
      </ItemGroup>
    </Project>
```

> O __Ropository (generic)__ e o __UnitOfWork Patterns__ foram criados com suas __Abstrações e Classes Concretas__. Abaixo o código:

#### GenericRepository - Abstração em Domain Layer

```CSharp
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    namespace Bookstore.Domain.Abstractions.Repository.Base
    {
        public interface IGenericRepository<T,Key> : IDisposable where T : class
        {
            Task<T> GetById(Key id);
            Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null);
            Task Add(T obj);
            Task Update(T obj);
            Task Delete(T obj);
        }
    }
```

#### GenericRepository - Implementação em Infra/Data Layer

```CSharp
    using Bookstore.Domain.Abstractions.Repository.Base;
    using Bookstore.Infra.Data.Orm;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    namespace Bookstore.Infra.Repository.Base
    {
        public abstract class GenericRepository<T, Key> : IGenericRepository<T, Key> where T : class, new()
        {
            protected ApplicationDbContext _context;
            public GenericRepository(ApplicationDbContext context) => _context = context;

            public virtual async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null)
            {
                if (predicate == null) return await _context.Set<T>().AsNoTracking().ToListAsync();
            
                return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
            }
            public virtual async Task<T> GetById(Key id)
            {
                return await _context.Set<T>().FindAsync(id);
            }
            public virtual async Task Add(T obj)
            {
                _context.Set<T>().Add(obj);
                await Task.CompletedTask;
            }
            public virtual async Task Update(T obj)
            {
                _context.Set<T>().Update(obj);
                await Task.CompletedTask;
            }
            public virtual async Task Delete(T obj)
            {
                _context.Set<T>().Remove(obj);
                await Task.CompletedTask;
            }
            public void Dispose()
            {
                _ = (_context?.DisposeAsync());
            }
        }
    }

```

#### UnitOfWork - Abstração em Domain Layer

```CSharp
    using System.Threading.Tasks;
    namespace Bookstore.Domain.Abstractions.DomainInterfaces
    {
        public interface IUnitOfWork
        {
            Task<bool> Commit();
            Task Rollback();
        }
    }
```

#### UnitOfWork - Composição com IRepositoryProducts em Domain Layer too.

```CSharp
    using Bookstore.Domain.Abstractions.DomainInterfaces;
    using Bookstore.Domain.Abstractions.Repository.Base;
    using Bookstore.Domain.Entities;
    namespace Bookstore.Domain.Abstractions.Repository
    {
        public interface IRepositoryProducts : IUnitOfWork, IGenericRepository<Product, int>
        {
        }
    }
```

> Rotinas de __Atualização__ (HttpPut) e __Exclusão__ (HttpDelete) foram adicionas ao projetos, pois as mesmas não constavam no __projeto original__.

> Todos os métodos do nosso GenericRepository são __virtual__ async. Isso nos possibilita sobrecarregá-los quando necessário. Usamos essa abordagem para usar o __Polimorfismo__ quando demos override no método GetById em nosso RepositoryProduct, pois o projeto original seta o Id de product como __string__ e recebemos um __int__ na Action HttpGet => GetById. Observe o código abaixo:

```Csharp
    using Bookstore.Domain.Abstractions.Repository;
    using Bookstore.Domain.Entities;
    using Bookstore.Infra.Data.Orm;
    using Bookstore.Infra.Repository.Base;
    using System.Threading.Tasks;

    namespace Bookstore.Infra.Repository.Entities
    {
        public class RepositoryProducts : GenericRepository<Product, int>, IRepositoryProducts
        {
            public RepositoryProducts(ApplicationDbContext ctx) : base(ctx) => _context = ctx;

            public async Task<bool> Commit() 
            {
                return await _context.SaveChangesAsync() > 0;
            }
            public async Task Rollback()
            {
                /* To do any process... Call other implementation, etc */
                await Task.CompletedTask;
            }
            public async override Task<Product> GetById(int id)
            {
                var idStr = id.ToString();
                return await _context.Set<Product>().FindAsync(idStr);
            }
        }
    }
```

> Trocamos nosso __"Acesso a Dados"__, usando o MS SQLServer localDb no lugar do acesso com UseInMemomy;


> A API está totalmente funcional e disponível para testes através do Swagger:


<img src="https://github.com/carlosItDevelop/criando-seu-ecommerce-de-livros-csharp-e-angular/blob/main/imgs/DIO-v3-animada.gif" height="475" >'


- DI <IRepositoryProducts, RepositoryProducts> in Startup Scoped Life Cicle <= Inversion Of Control;
- Add Attributes ProducesResponseType(typeof(Product), StatusCodes.Status201Created and StatusCodes.Status400BadRequest;
- SeedData Class with Extension Method Initializer created and Program.cs;
- Registered services.AddAutoMapper(typeof(AutoMapperConfig)) in Startup Class;
- Mapper.Map<>() Product/ProductDTO > Reverse Implemented in ProductController;
- ConfigureSwaggerOptions, Versioning and DefaultValues in Extensions Methods;
- services.AddSwaggerConfig() and app.UseSwaggerConfig(provider) Extension Methods created;
- services.AddTransient<IConfigureOptions<SwaggerGenOptions>, Configured;
- Copy BookstoreController to v1, v2 and v3 and MainController created in Root/Controllers;
- Versioned Controllers inherited from MainController and without ApiController Attributes;
- ApiVersion and Router Changes in Controllers v1, v2 and v3;
- v1 with GetAll only, v2 with GetAll and GetById and v3 with all Methods and v1 marked as obsolete (Deprecated);
- ProductMap created in Infra/Mappings and ApplyConfiguration and SetColumnType in OnModelCreating;

---

> Este é um resumo dos principais Commits realizados no Upgrade do projeto para a Versão 1.1.0.  No README do Projeto estarão resumidas as funcionalidades implementadas e as alterações e correções de bugs.

> _IMPORTANTE:_ Apenas o Backend foi modificado e testado com o Swagger. 

## Reporting security issues and bugs

Security issues and bugs should be reported privately, via email, to Cooperchip (SAC) at <carlos.itdevelop@gmail.com>. You should receive a response within 72 hours. If for some reason you do not, please follow up via email to ensure we received your original message.

## Code of conduct

This project has adopted the [Cooperchip Open Source Code of Conduct](https://cooperchip.com.br/).  For more information, see the [Code of Conduct FAQ](https://cooperchip.com.br/) or contact [carlos.itdevelop@gmail.com](mailto:carlos.itdevelop@gmail.com) with any additional questions or comments.

---

> ### Versão 2 da API com GetById e GetAll com Repository Pattern e UnitOfWork Pattern implementados


![Projeto DIO API e-Commerce com Angular e CSharp-v2 -  Versionado](https://github.com/carlosItDevelop/criando-seu-ecommerce-de-livros-csharp-e-angular/blob/main/imgs/api-v2.png "API Versionada - V2")

> ### Versão 3 da API com CRUD completo e todos os Patterns implementados

![Projeto DIO API e-Commerce com Angular e CSharp-v3 - Versionado](https://github.com/carlosItDevelop/criando-seu-ecommerce-de-livros-csharp-e-angular/blob/main/imgs/api-v3.png "API Versionada - V3, com CRUD completo")

### Novas demandas:

- [ ] Instalar, Configurar e Registrar o FluentValidation;
	- [ ] Criar o ProductValidation e o DomainService;
	- [ ] Validar com BaseService e Notification Pattern;

- [ ] Upload do Arquivo de Imagem do Product;
	- [ ] Fazer isso também no Swagger;

- [ ] Separar Category em uma nova Model;
    - [ ] Criar e mapear o relacionamento entre Product e Category;


### Features Required && Implemented

- :white_check_mark: Onion Architecture                            
- :black_square_button: CQRS with MediatR Library                     
- :white_check_mark: Entity Framework Core - Code First
- :white_check_mark: Repository Pattern - Generic
- :black_square_button: MediatR Pipeline Logging & Validation
- :white_check_mark: Swagger UI
- :white_check_mark: Healthchecks
- :black_square_button: Pagination
- :black_square_button: In-Memory Caching
- :black_square_button: Redis Caching
- :black_square_button: Microsoft Identity with JWT Authentication
- :white_check_mark: Role based Authorization
- :black_square_button: Identity Seeding
- :white_check_mark: Database Seeding
- :black_square_button: Custom Exception Handling Middlewares
- :white_check_mark: API Versioning
- :black_square_button: Fluent Validation
- :white_check_mark: Automapper
- :black_square_button: SMTP / Mailkit / Sendgrid Email Service
- :black_square_button: Complete User Management Module (Register / Generate Token / Forgot Password / Confirmation Mail)
- :black_square_button: User Auditing