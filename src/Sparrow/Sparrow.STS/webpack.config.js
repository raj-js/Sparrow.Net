var path = require('path');

module.exports = {
    entry: {
        main: 'index.js'
    },
    output: {
        path: path.resolve(__dirname, '/www/js'),
        filename: 'bundle.js'
    }
};
