const purgecss = require('@fullhuman/postcss-purgecss')({
    content: [
        './View/**/*.cshtml',
    ],
    defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || [],
});

module.exports = {
    plugins: {
        tailwindcss: {},
        autoprefixer: {},
        ...(process.env.NODE_ENV === 'production' ? { purgecss } : {}),
    },
}
