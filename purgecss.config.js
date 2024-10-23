const path = require('path');

module.exports = {
    content: [
        path.resolve(__dirname, 'Views/**/*.cshtml'),  
    ],
    css: [
        path.resolve(__dirname, 'wwwroot/site.css')   
    ],
    output: path.resolve(__dirname, 'wwwroot/css'),  
    safelist: ['html', 'body', 'container'],         
    defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || [],
};
