module.exports = {
    content: [
        "./Views/**/*.{cshtml,js}",
        "./wwwroot/**/*.{html,js}",
    ],

    theme: {
        extend: {
            colors: {
                stromboli: '#366857',
                downy: '#6eccac',
                edward: '#abb7b4',
                seafoamgreen: '#93e9be',
                applegreen: '#def2ed',
            },
            fontFamily: {
                sans: ['Nunito', 'Inter', 'sans-serif'],
            },
        },
    },
};
