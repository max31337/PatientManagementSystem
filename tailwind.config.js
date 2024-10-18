/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Views/**/*.{cshtml,html}",
        "./wwwroot/**/*.{html,js}",
    ],
    theme: {
        extend: {
            colors: {
                stromboli: '#426A5A',
                downy: '#6FCF97',
                edward: '#A9BDBA',
                appleGreen: '#71B340',
            },
            fontFamily: {
                sans: ['Nunito', 'Inter', 'sans-serif'],
            },
        },
    },
    plugins: [],
};
