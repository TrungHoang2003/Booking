# Booking.com Clone - Login Page

Trang đăng nhập được xây dựng với Next.js 14 và Tailwind CSS thuần túy, thiết kế theo chuẩn Booking.com.

## Cấu trúc thư mục

```
src/
├── app/
│   ├── globals.css          # CSS toàn cục với Booking.com theme
│   ├── layout.tsx          # Layout chính
│   ├── page.tsx            # Trang chủ với hero section
│   └── login/
│       ├── layout.tsx      # Layout cho trang login
│       └── page.tsx        # Trang đăng nhập
├── components/
│   ├── auth/
│   │   ├── login-form.tsx         # Form đăng nhập chính
│   │   ├── login-header.tsx       # Header cho trang login
│   │   └── social-login-buttons.tsx # Buttons đăng nhập social
│   └── ui/
│       ├── button.tsx      # Component Button có thể tái sử dụng
│       └── input.tsx       # Component Input có thể tái sử dụng
└── lib/
    └── utils.ts            # Utilities functions
```

## Tính năng chính

### 🎨 Giao diện
- **Thiết kế đúng chuẩn Booking.com**: Màu sắc, typography, spacing
- **Responsive**: Tự động điều chỉnh trên mobile/tablet/desktop
- **Tailwind CSS thuần túy**: Không dependency phụ thuộc
- **Loading states**: Hiển thị trạng thái loading khi submit

### 🔐 Authentication
- **Email login**: Form đăng nhập bằng email
- **Social login**: Google, Apple, Facebook (UI đã sẵn sàng)
- **Form validation**: Validation HTML5 cơ bản
- **Error handling**: Cấu trúc sẵn sàng cho error display

### 🏗️ Architecture
- **Component-based**: Chia nhỏ thành các components tái sử dụng
- **TypeScript**: Type safety cho toàn bộ dự án
- **Next.js 14**: App Router, Server Components
- **Modern CSS**: Flexbox, Grid, CSS Variables

## Các màu sắc chính

```css
/* Booking.com Brand Colors */
--booking-blue: #003580;        /* Primary blue */
--booking-light-blue: #0071c2;  /* Light blue for buttons */
--booking-hover-blue: #005bb5;  /* Hover states */
```

## Chạy dự án

```bash
# Cài đặt dependencies
npm install

# Chạy development server
npm run dev

# Build production
npm run build
```

## Truy cập

- **Trang chủ**: http://localhost:3000
- **Trang đăng nhập**: http://localhost:3000/login

## Tính năng có thể mở rộng

1. **Authentication API**: Kết nối với backend
2. **Form validation**: React Hook Form + Zod
3. **State management**: Redux/Zustand
4. **OAuth integration**: NextAuth.js
5. **Multi-language**: i18n support
6. **Dark mode**: Theme switching
7. **Testing**: Jest + Testing Library
8. **Analytics**: Google Analytics integration

## Components chi tiết

### LoginForm
- Email input với validation
- Submit button với loading state
- Social login integration
- Terms & conditions links

### LoginHeader
- Booking.com logo
- Language selector
- Help/support button

### SocialLoginButtons
- Google OAuth button
- Apple OAuth button
- Facebook OAuth button
- Hover effects và accessibility

## Responsive Design

- **Mobile**: Stack layout, full-width inputs
- **Tablet**: Optimized spacing, centered form
- **Desktop**: Full width layout, optimal UX

## Accessibility

- **Keyboard navigation**: Tab-friendly
- **Screen reader**: ARIA labels
- **Color contrast**: WCAG compliant
- **Focus states**: Visible focus indicators
