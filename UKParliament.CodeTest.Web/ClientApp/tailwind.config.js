/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}"
  ],
  theme: {
    extend: {
      colors: {
        'purple': {
          pale: '#efebf5',
          light: '#706982',
          DEFAULT: '#625a75',
          dark: '#373151'
        },
        'grey': '#ebe9e8',
        'surface': '#ffffff',
        'white': '#ffffff',
      },
      fontSize: {
        lg: '1.2rem',
        xl: '1.5rem'
      }
    },
  },
  plugins: [],
}

