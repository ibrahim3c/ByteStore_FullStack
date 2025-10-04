---
# Design System: "ElectroModern" for E-commerce (Angular Material)

## 1. Project Goal & Vibe
- **Vibe:** Modern, clean, trustworthy, professional, and tech-focused.
- **Goal:** Create a seamless user experience for browsing and purchasing electronic devices. The design should highlight product details and build customer confidence.
- **Note:** All custom styles will be written in CSS (not SCSS).

## 2. Color Palette (For Angular Material Theming)
- **Primary Color (Branding, main buttons, active links):**
  - Hex: `#0D47A1` (Deep Blue)
  - Description: Suggests trust and technology.
- **Accent Color (Call-to-action buttons like 'Add to Cart', highlights):**
  - Hex: `#00ACC1` (Bright Cyan/Teal)
  - Description: Vibrant and eye-catching for important actions.
- **Warn Color (Errors, warnings):**
  - Hex: `#D32F2F` (Standard Red)
- **Surface & Background Colors:**
  - **Background:** `#F5F7FA` (Very light, slightly cool gray) - For the main page background.
  - **Surface:** `#FFFFFF` (White) - For components like Cards, Dialogs, and Toolbars.
- **Text Colors:**
  - **Primary Text:** `#212121` (Almost Black) - For headings and primary content.
  - **Secondary Text:** `#757575` (Medium Gray) - For details and helper text.
  - **Hint Text:** `#BDBDBD` (Light Gray) - For placeholders in form fields.

## 3. Typography (Based on Google Fonts - Roboto)
- **Font Family:** 'Roboto', sans-serif.
- **Type Scale:**
  - `H1 (display-2)`: 48px, Bold (700) - For main product titles on detail pages.
  - `H2 (headline)`: 36px, Bold (700) - For main section titles.
  - `H3 (title)`: 24px, Medium (500) - For product titles in cards.
  - `Body-1`: 16px, Regular (400) - For descriptions and long-form text.
  - `Body-2`: 14px, Regular (400) - For secondary details and specs.
  - `Button`: 14px, Medium (500), All caps (UPPERCASE).

## 4. Sizing, Spacing & Layout
- **Base Spacing Unit:** 8px grid system. All margins and paddings are multiples of 8 (e.g., 8px, 16px, 24px, 32px).
- **Border Radius:**
  - **General (Buttons, Cards, Inputs):** `8px` for a modern, soft look.
  - **Images:** `12px` for product images.
- **Layout:** Standard 12-column responsive grid.

## 5. Component-Specific Styles (Customizing Angular Material)
- **Buttons (`mat-raised-button`, `mat-stroked-button`):**
  - **Primary Actions ('Login', 'Checkout'):** Use `<button mat-raised-button color="primary">`.
  - **Call-to-Action ('Add to Cart'):** Use `<button mat-raised-button color="accent">`.
  - **Secondary Actions ('View Details'):** Use `<button mat-stroked-button color="primary">`.
- **Cards (`mat-card`):**
  - `background-color`: Surface color (`#FFFFFF`).
  - `box-shadow`: `0 4px 12px rgba(0,0,0,0.08)`.
  - `padding`: `24px`.
- **Input Fields (`mat-form-field`):**
  - Use `appearance="outline"` with a border-radius of `8px`.
- **Chips (`mat-chip-list`):**
  - Use for product tags (e.g., "SALE", "NEW", "16GB RAM").
  - Default chip style uses the `primary` color for its selected state. Use `<mat-chip color="accent" selected>` for special offers.
- **Toolbar (`mat-toolbar`):**
  - `background-color`: Surface color (`#FFFFFF`).
  - `border-bottom`: `1px solid #E0E0E0`.
---
