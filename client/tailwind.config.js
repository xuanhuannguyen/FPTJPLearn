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
          tertiary: 'var(--text-tertiary)',
        },
        accent: {
          primary: 'var(--accent-primary)',
          hover: 'var(--accent-hover)',
          success: 'var(--accent-success)',
          danger: 'var(--accent-danger)',
          warning: 'var(--accent-warning)',
          cta: 'var(--accent-cta)',
          info: 'var(--accent-info)',
        },
        border: 'var(--border-color)',
      },
      fontFamily: {
        main: ['"Nunito Sans"', 'sans-serif'],
        jp: ['"Noto Sans JP"', 'sans-serif'],
        heading: ['"Baloo 2"', 'sans-serif'],
      },
      boxShadow: {
        card: 'var(--shadow-card)',
        lift: 'var(--shadow-lift)',
        glow: 'var(--shadow-glow)',
        'glow-sm': '0 8px 18px rgba(13, 148, 136, 0.18)',
        'glow-lg': '0 20px 44px rgba(13, 148, 136, 0.26)',
        warm: 'var(--shadow-warm)',
        pop: 'var(--shadow-pop)',
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
