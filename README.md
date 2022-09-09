# MVC-B-API-ID-Template
 
## What is it?

An Asp.Net Core 6.0 Template made for small projects that do not need to be based of off DDD for my personal use.

![image](https://user-images.githubusercontent.com/49447848/189417096-97ca0eae-018f-4a88-a026-d2d63c66aa32.png)

## It is built using:
- Model-View-Controller Server
  - with Authorization
  - with Blazor WASM Support

- WebApi 
  - with Authorization
  - Request/Response Logging Middleware to SQLLite Database
  - AppDbContext ready with BaseEntity class with CreatedDate and UpdatedDate support

- Identity Server 4
  - Supporting both Api and MVC Scope Authorization with predefined MVCScope with access to the WebApi

- Blazor WASM Client
  - Preconnected to the MVC Server for use of components
    

## To Be Added

  ### Server Based
  - [ ] Add Easily Removable Localization
  - [ ] Look into Identity Server 4's ability to perform role based authentication based on data stored on it's system and check for outside libraries able to ease its use

  ### Api Centered
- [ ] Separate reused models across WebApi and Server into separate project 
- [ ] Implement DTO mapper for WebApi
  - Possible usage of [AutoMapper](https://docs.automapper.org/en/stable/Getting-started.html) with Profile based configuration for ease of use
- [ ] Typesafe Rest Api Client Library
  - Possible usage of [Restless](https://github.com/letsar/RestLess, "Compilation Time Generated Rest Api Client Library") 
- [ ] Check for libraries implementing Repository<Type id, Type model> for ease of use of DbContext
- [ ] Add easy database manipulation for WebApi databases for testing purposes
  - Possible usage of [Core-Admin](https://github.com/edandersen/core-admin) Automatic Crud Generation
    - Need to previously either establish role-based authorization or use `app.Environment.IsDevelopment();`
- [ ] Implement Model Validation and implement it in Db manipulation methods or check it using Middleware and block requests with invalid models
  - Possible usage of [FluentValidation](https://github.com/FluentValidation/FluentValidation). Library that gives the ability to write AbstractValidators for models 

