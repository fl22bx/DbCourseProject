const path = require('path')

module.exports = {
  entry: './Index.js',
  output: {
    filename: 'build.js',
    path: path.resolve(__dirname, "dist")
  },
       module: {
         rules: [
             {
                 test: /\.js$/,
                 loader: 'babel-loader',
             }
         ]
     }
}
