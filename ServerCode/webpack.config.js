const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const TerserPlugin = require('terser-webpack-plugin');

module.exports = {
    mode: "production",
    optimization: {
        minimize: true,
        minimizer: [new TerserPlugin({
            parallel: true,
            cache: true,
        })],
    },
    
    // Enable sourcemaps for debugging webpack's output.
    devtool: "source-map",

    resolve: {
        // Add '.ts' and '.tsx' as resolvable extensions.
        extensions: [".ts", ".tsx", ".js"]
    },

    output: {
        path: path.resolve(__dirname, 'public/builds')
    },
    devServer: {
        contentBase: path.join(__dirname, 'public'),
        compress: true,
        port: 9000
      },
    
    module: {
        rules: [
            {
                test: /\.ts(x?)$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: "ts-loader"
                    }
                ]
            },
            // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
            {
                enforce: "pre",
                test: /\.js$/,
                loader: "source-map-loader"
            },
            {
                test: /\.(scss|sass|css)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "sass-loader"
                ]
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: './css/[name].css'
        })
    ],
    // When importing a module whose path matches one of the following, just
    // assume a corresponding global variable exists and use that instead.
    // This is important because it allows us to avoid bundling all of our
    // dependencies, which allows browsers to cache those libraries between builds.
    externals: {
        "xlsx": "XLSX",
    }
};