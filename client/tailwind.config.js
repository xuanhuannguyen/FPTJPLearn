/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        bg: {
          primary: 'var(--bg-primary)',
          secondary: 'var(--bg-secondary)',
          tertiary: 'var(--bg-tertiary)',
        },
        text: {
          primary: 'var(--text-primary)',
          secondary: 'var(--text-secondary)',
          muted: 'var(--text-muted)',
        },
        accent: {
          primary: 'var(--accent-primary)',
          hover: 'var(--accent-hover)',
          success: 'var(--accent-success)',
          danger: 'var(--accent-danger)',
          warning: 'var(--accent-warning)',
        },
        border: 'var(--border-color)',
      },
      fontFamily: {
        main: ['"Nunito Sans"', 'sans-serif'],
        jp: ['"Noto Sans JP"', 'sans-serif'],
        heading: ['"Varela Round"', 'sans-serif'],
      },
      boxShadow: {
        card: '0 4px 20px rgba(0, 0, 0, 0.05)',
        glow: '0 0 15px rgba(37, 99, 235, 0.2)',
      },
      borderRadius: {
        xl: '16px',
        '2xl': '24px',
      },
      keyframes: {
        shake: {
          '0%, 100%': { transform: 'translateX(0)' },
          '10%, 30%, 50%, 70%, 90%': { transform: 'translateX(-5px)' },
          '20%, 40%, 60%, 80%': { transform: 'translateX(5px)' },
        }
      },
      animation: {
        shake: 'shake 0.4s ease-in-out',
      }
    },
  },
  plugins: [],
}
