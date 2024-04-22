# ProductApi
ProductApi is a .NET 6 API with various endpoints enabling the Create, Read, Update and Delete (CRUD) operations.

## Source Code
[GitHub](https://github.com/PriyankKothari/ProductApi)

## Getting Started
- Clone the repository:
git clone https://github.com/PriyankKothari/ProductApi.git
  - Visual Studio
    - Open solution (ProductApi.sln) in Visual Studio
    - Build solution
    - Debug solution.
    - Use Postman (**recommended**) to run different endpoints such as:
      - (HTTPGET) https://localhost:7290/v1/products/ - Lists all products
      - (HTTPPOST) https://localhost:7290/v1/products/ - Creates a product
      - (HTTPPUT) https://localhost:7290/v1/products/ - Updates a product
      - (HTTPDELETE) https://localhost:7290/v1/products/ - Deletes a product
    - You can also try following HTTPGET endpoints:
      - https://localhost:7290/v1/products/{id:int} - Gets a product with matching Id
      - https://localhost:7290/v1/products/{name} - Gets a product with matching name
      - https://localhost:7290/v1/products/brands/{brandName} - Lists all products with matching brand name
      - https://localhost:7290/v1/products/{name}/brands/{brandName} - Gets a product with matching name and brand name
  - Use Swagger documentation (UI interface)
    - Several limitations using the Swagger UI interface as Swagger tries to avoid calling wrong endpoint.
      - For example, https://localhost:7290/v1/products/{id:int} doesn't have 'id' parameter mandetory. But if not provided, the endpoint to 'List all products' will get executed. Hence it is 'Required'.
  
## Imrpovements / TODO
- Detailed Swagger Documentation
- Extensive logging such as log a method exeuction (as soon as control enters a method)
- Detailed logging such as time taken for an execution of method.
- Unit tests to cover scenarios such as:
  - execution of logger (if logging was executed)
  - exception occurance
  - mapping of From and To objects
