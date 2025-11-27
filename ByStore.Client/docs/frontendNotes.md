-----

# ByteStore – Frontend Project Documentation (Angular)

## 1\) Executive Summary

The ByteStore Frontend is a modern, responsive, and performant single-page application (SPA) built with Angular. It will serve as the client-side counterpart to the ByteStore backend API, providing a rich user experience for both customers and administrators. The application will be modular, scalable, and maintainable, following best practices for Angular development.

-----

## 2\) Goals & Non‑Functional Requirements

  * **Responsiveness:** A mobile-first design that adapts seamlessly across all devices (desktop, tablet, mobile).
  * **Performance:** Fast initial load times (lazy loading), efficient runtime performance (OnPush change detection), and a smooth user experience.
  * **User Experience (UX):** Intuitive navigation, clear user feedback (loaders, snackbars for errors/success), and a streamlined checkout process.
  * **Accessibility (a11y):** Adherence to WCAG guidelines to ensure the application is usable by people with disabilities.
  * **Maintainability:** A clean, modular codebase with a clear separation of concerns, strong typing, and consistent coding standards.
  * **Security:** Secure handling of JWTs, protection against XSS, and proper route guarding.

-----

## 3\) Architecture & Project Structure

The application will use a modular architecture, separating features into their own `NgModules`. This enables lazy loading and a clear separation of concerns.

**Proposed Folder Structure:**

```
/bytestore-app
  /src
    /app
      /core
        /guards/              // Route guards (AuthGuard, AdminGuard)
        /interceptors/        // HTTP interceptors (JwtInterceptor, ErrorInterceptor)
        /models/              // Global interfaces (ApiResponse, User, Product)
        /services/            // Global singletons (AuthService,   ApiService, CartService)
        /state/               // Optional: global state (signals, NgRx, etc.)
        /components/          // Global layout components (Navbar, Footer)
      /features
        /auth/
          login/
          register/
          auth.routes.ts
        /cart/
          cart.component.ts
          cart.routes.ts
        /products/
          product-list/
          product-details/
          catalog.routes.ts
        /checkout/
          checkout.component.ts
          checkout.routes.ts
        /orders/
          orders.component.ts
        /account/
          profile/
          orders/
          account.routes.ts
        /admin/
          dashboard/
          products/
          orders/
          admin.routes.ts
      /shared/
        /components/          // Reusable UI widgets (Button, Spinner, Modal)
        /directives/          // Shared directives
        /pipes/               // Reusable pipes (currency, filter)
        /validators/          // Custom validators
        /utils/               // Helper functions
      app.component.ts
      app.component.html
      app.component.scss
      app.routes.ts
      app.config.ts
    /assets/
      /icons/
      /images/
    /environments/
      environment.ts
    main.ts
    style.css
```

-----

## 4\) State Management

For an e-commerce application with a shopping cart, user session, and complex product filters, a robust state management solution is recommended.

  * **Primary Recommendation: NgRx (or similar like Elf, Akita)**
      * **Store:** A single source of truth for application state (e.g., `cart`, `user`, `products`).
      * **Actions:** Describe unique events that happen in the application (e.g., `[Cart] Add Item`, `[Auth] Login Success`).
      * **Reducers:** Pure functions that handle state transitions based on actions.
      * **Effects:** Handle side effects like API calls. For example, an `[Order] Place Order` action would trigger an effect to call the `OrderService`.
      * **Selectors:** Memoized functions to efficiently query and derive data from the store.

This provides a predictable, debuggable, and scalable way to manage application state.

-----

## 5\) Core Modules & Services

### `CoreModule` (Imported once in `AppModule`)

  * **`AuthService`:** Manages the entire authentication lifecycle: login, logout, registration, token storage, and decoding JWTs to get user roles and info.
  * **`ApiService`:** A generic wrapper around Angular's `HttpClient` for making requests to the backend. It will handle setting base URLs from environment files.
  * **`StorageService`:** An abstraction for interacting with `localStorage` or `sessionStorage` to securely store the refresh token and user preferences.
  * **`JwtInterceptor`:** Attaches the JWT access token to the `Authorization` header of all outgoing HTTP requests.
  * **`ErrorInterceptor`:** Catches global HTTP errors. It will parse the RFC 7807 `ProblemDetails` from the backend and display user-friendly messages using a notification service (e.g., Angular Material Snackbar). It will also handle 401 Unauthorized errors to trigger the token refresh flow.

### `SharedModule` (Imported by feature modules)

  * Contains common UI components (`SpinnerComponent`, `ModalComponent`), pipes, and directives that are used across multiple feature modules. It will export `CommonModule` and `FormsModule`/`ReactiveFormsModule`.

-----

## 6\) Authentication Flow

1.  **Login:** User submits credentials to a login form. `AuthService.login()` is called.
2.  **Token Reception:** The service receives a JWT access token and a refresh token from the backend.
3.  **Storage:**
      * The **access token** is stored in memory within the `AuthService`.
      * The **refresh token** is stored securely in `localStorage` via the `StorageService`.
4.  **Request Authorization:** The `JwtInterceptor` automatically attaches the access token to all subsequent API requests.
5.  **Token Expiration (401 Error):**
      * When the access token expires, the API will return a `401 Unauthorized` status.
      * The `ErrorInterceptor` catches this `401`.
      * It calls `AuthService.refreshToken()`, which sends the refresh token to the `/api/v1/auth/refresh` endpoint.
      * If successful, a new access token is received and stored. The original failed request is transparently re-tried with the new token.
      * If the refresh token is also invalid, the user is logged out and redirected to the login page.
6.  **Route Protection:**
      * `AuthGuard`: Prevents unauthenticated users from accessing protected routes (e.g., `/account`, `/checkout`).
      * `AdminGuard`: Prevents non-admin users from accessing the `/admin` feature module.

-----

## 7\) Data Models (TypeScript Interfaces)

Interfaces will be created in `/core/models` to match the backend DTOs and entities, ensuring type safety throughout the application.

**Example: `product.model.ts`**

```typescript
export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  isActive: boolean;
  categoryId: number;
  brandId: number;
  categoryName: string; // Denormalized for display
  brandName: string;    // Denormalized for display
  images: ProductImage[];
  // ... other properties
}

export interface ProductImage {
  id: number;
  imageUrl: string;
  isPrimary: boolean;
  title: string;
}
```

Similar interfaces will be defined for `Category`, `Brand`, `Order`, `OrderItem`, `Cart`, `CartItem`, `User`, etc.

-----

## 8\) Feature Modules & Component Strategy

Each feature will be a lazy-loaded module containing its own routing, components, and services.

  * **`CatalogModule`**
      * **Services:** `CatalogService` (fetches products, categories, brands).
      * **Smart Components:** `ProductListComponent` (manages state for filtering, sorting, pagination, and fetching data).
      * **Presentational Components:** `ProductCardComponent` (receives a `Product` object via `@Input()` and displays it), `FilterSidebarComponent` (emits filter changes via `@Output()`).
  * **`CheckoutModule`**
      * **Services:** `CheckoutService`, `StripeService` (a wrapper for the Stripe.js library).
      * **Components:** A multi-step wizard component for Shipping Address, Billing Address, and Payment.
      * **Stripe Integration:** The payment component will:
        1.  Call the backend to create a payment intent via `PaymentService`.
        2.  Receive the `clientSecret` from the backend.
        3.  Use the `clientSecret` to initialize and mount Stripe Elements (secure card input fields).
        4.  On submission, use Stripe.js to confirm the card payment with the `clientSecret`.
        5.  On successful payment confirmation from Stripe, call the backend's `OrderService` to finalize placing the order.
  * **`AdminModule`** (Lazy Loaded, protected by `AdminGuard`)
      * Will contain components for CRUD operations on products, categories, etc., likely using Angular Material Table and Forms.

-----

## 9\) Styling

  * **Framework:** **Angular Material** or **PrimeNG** is recommended to provide a rich set of pre-built, accessible UI components.
  * **CSS Preprocessor:** **SCSS** for its advanced features like variables, nesting, and mixins.
  * **Structure:** A global stylesheet (`styles.scss`) for theme variables and base styles, with component-specific styles encapsulated within each component's `.scss` file.

-----

## 10\) Testing Strategy

  * **Unit Tests (Karma & Jasmine):**
      * Test services, pipes, and component logic in isolation.
      * Use `HttpClientTestingModule` to mock HTTP requests for services.
      * Mock service dependencies in components using stubs or spies.
  * **End-to-End (E2E) Tests (Cypress or Playwright):**
      * Test critical user flows from start to finish, such as the full login -\> add to cart -\> checkout flow. These tests run against a live (or mock) backend.

-----

## 11\) Environment & Configuration

The `src/environments` folder will manage configuration.

**`environment.ts` (for development):**

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001/api/v1',
  stripePublishableKey: 'pk_test_...'
};
```

**`environment.prod.ts` (for production):**

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.bytestore.com/api/v1',
  stripePublishableKey: 'pk_live_...'
};
```

-----

## 12\) Delivery Roadmap

This roadmap aligns with the backend delivery plan.

1.  **Foundation:** Setup Angular project, install dependencies, implement `CoreModule` (auth flow, interceptors), and basic layout.
2.  **Catalog:** Build product list and detail pages. Implement filtering, sorting, and pagination.
3.  **Cart:** Implement state management for the cart. Build components to view, add, update, and remove cart items.
4.  **Account:** Create login, registration, and user profile pages (order history, address management).
5.  **Checkout & Payment:** Implement the multi-step checkout flow with Stripe Elements integration for payments.
6.  **Admin Portal:** Build the lazy-loaded admin module for managing store data.
7.  **Refinement:** Implement caching (e.g., using `shareReplay`), add unit/E2E tests, and perform performance optimizations.



# To Dos
  organize routes and use lazy loading too
  refresh token does not work
