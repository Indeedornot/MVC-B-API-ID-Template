# MVC-B-API-ID-Template

## What is it?

An Asp.Net Core 6.0 Template made for small projects that do not need to be based of off DDD for my personal use.

![image](https://user-images.githubusercontent.com/49447848/189417096-97ca0eae-018f-4a88-a026-d2d63c66aa32.png)

## It is built using

- MVC Server

    - with User Account Authorization
        - and role authorization
        - with access to user data using `id_token`
    - with Blazor WASM Components Support
    - with Refit for Easy API Calls integrated with ID4 `access_token` for Authentication
    - With Shared and Local Localization

- WebApi

    - with Authorization and Authentication using ID4
        - Role based authorization with the ability to add different schemas for different endpoints using Policies
    - With Swagger configured for easy testing with the ability to authenticate to access protected endpoints
    - Request/Response Logging Middleware to SQLLite Database
    - AppDbContext ready with BaseEntity class with CreatedDate and UpdatedDate support
    - with FastEndpoints for clean Response/Request Endpoints
    - ~~With Shared and Local Localization~~
        - (Not yet implemented culture detection based on the request)

- Identity Server 4

    - Supporting both Api and MVC Scope Authorization with predefined MVCScope with access to the WebApi
    - With SQLLite Usage storage setup
    - With easily modifiable TestUsers for easy testing and a seeding setup

- Blazor WASM Client
    - Preconnected to the MVC Server for use of components
    - With Sass support
    - With Shared and Local Localization

## To Be Added

### Server Centered

- [x] ~~Look into Identity Server 4's ability to perform role based authentication based on data stored on it's system
  and
  check for outside libraries able to ease its use~~
- [x] ~~Fix all claims being added to the access token~~
    - Add default role
- [ ] Create Views explaining each feature of this template
- [x] ~~Add Identity Server service for claim retrieval~~
    - ~~Should be temporary fix if other options work~~
    - Proper usage of OAuth2.0 and OpenID Connect fixed the issue
- [ ] Add Theming Support
- [x] Add Localization Support

### Api Centered

- [ ] Implement DTO mapper for WebApi
    - Possible usage of [AutoMapper](https://docs.automapper.org/en/stable/Getting-started.html) with Profile based
      configuration for ease of use
- [x] ~~Typesafe Rest Api Client Library~~
    - ~~Possible usage
      of [Restless](https://github.com/letsar/RestLess "Compilation Time Generated Rest Api Client Library")~~
- [x] ~~Check for libraries implementing `Repository<Type id, Type model>` for ease of use of DbContext~~
    - Not needed due to EFCore implementing Repository pattern - Use Generic Crud methods on DbContext
- [ ] Look into EFCore DbConnection type for ease of use of DbContext and the ability to implement UnitOfWork pattern
- [x] ~~Add easy database manipulation for WebApi databases for testing purposes~~
    - ~~Possible usage of [Core-Admin](https://github.com/edandersen/core-admin) Automatic Crud Generation~~
        - ~~Need to previously either establish role-based authorization or use `app.Environment.IsDevelopment();`~~
    - ~~Created Database recreation script - To Be Extended~~
- [ ] Implement Model Validation and implement it in Db manipulation methods or check it using Middleware and block
  requests with invalid models
    - Possible usage of [FluentValidation](https://github.com/FluentValidation/FluentValidation). Library that gives the
      ability to write AbstractValidators for models
- [ ] Log unauthorized requests to the WebApi using the Middleware
- [ ] Add Localization Support
    - Yet To Be Fully Finished
        - The ability to localize strings is already implemented
        - Need to add culture detection based on the request

### ID4 Centered

- [x] ~~Look into QuickStartUI Asp.Net Identity template for automatic claim insert into ClaimPrincipal in the
  MVCServer~~
- [ ] Add Registration
    - Although not supported by ID4 it is still possible to implement
        - tho with more work due to there being predefined services for similiar actions but not registering
- [ ] Add Localization Support

### Separation Needed

- [x] ~~Add Easily Removable Localization~~
    - ~~Possible usage of built
      in [Localization](https://www.codemag.com/Article/2009081/A-Deep-Dive-into-ASP.NET-Core-Localization) - Deep Dive
      into Localization~~
    - ~~Separation Needed to support both WebApi, MVC and Blazor WASM~~
- [x] ~~Separate reused models across WebApi and Server into separate project~~
