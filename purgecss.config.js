const glob = require('glob');
const path = require('path');

module.exports = {
    content: [
        path.join(__dirname, 'Views/**/*.cshtml'), 
        path.join(__dirname, 'Scripts/**/*.js'),   
    ],
    css: [
        path.join(__dirname, 'Content/**/*.css'),   
    ],
    output: './wwwroot/css/',
};
