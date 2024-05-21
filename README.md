# Wine Shop Management System

The Wine Shop Management System is a web-based application designed for employees of wine shops to efficiently manage financial records and inventory. Users can log in using their username and password.

## Functional Requirements

- **User Authentication**: Users should be able to log in with secure credentials to access the system based on their roles (Example: Admin).
- **Sales Management**: Enable users to record sales transactions, including product details, quantities, prices.
- **Inventory Management**: Allow users to track wine stock levels, including additions, deductions.
- **Purchase Tracking**: Enable users to record purchases from suppliers, including supplier details, purchase quantities, costs, and payment terms.
- **Expense Tracking**: Users should be able to record and categorize all expenses incurred by the wine shop, such as rent, utilities, salaries, and marketing.
- **Financial Reporting**: Provide users with comprehensive financial reports, including profit and loss statements, balance sheets, and cash flow statements.
- **User Management**: Allow administrators to manage user accounts, roles, and permissions to control access to system functionalities.

## User Roles

1. **Admin**
   - Add new Employees
   - View reports
2. **Purchase Manager**
   - Inventory management
   - Product Management
   - Suppliers
3. **Store Manager**
   - Orders
   - Expense Claims

## Login Module (JWT)

1. User enters username and password -> Server will verify the user (Authenticate)
2. Server will issue a Token with expire -> Client will store the token.
3. The stored token is sent in the request header every time -> Server checks the token and sends the Response.

## Technology Stack

- ASP.Net MVC front end 
- Web API
