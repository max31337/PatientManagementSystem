const path = require('path');

module.exports = {
    content: [
        path.resolve(__dirname, 'Views/**/*.cshtml'),  
        path.resolve(__dirname, 'wwwroot/js/*.js'),  
    ],
    css: [
        path.resolve(__dirname, 'wwwroot/css/site.css')   
    ],
    output: path.resolve(__dirname, 'wwwroot/css'),  
    safelist: ['html', 'body', 'container'],         
    defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || [],
};
