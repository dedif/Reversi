const gulp = require("gulp");
const { src, dest } = require("gulp");
const browserSync = require("browser-sync").create();
const sass = require("gulp-sass");
const order = require("gulp-order");
const babel = require("gulp-babel");
const uglify = require("gulp-uglify");
const cleanCSS = require("gulp-clean-css");
const autoprefixer = require("gulp-autoprefixer");
const concat = require("gulp-concat");
const pump = require("pump");

sass.compiler = require("node-sass");

const rootDir = "wwwroot/";

gulp.task("build", done => {
    _build(done);
});

function _build(done) {
    console.log("Running build task");
    done();
}

function _readJSFiles() {
    return src(rootDir + "src/js/**/*.js").pipe(dest(rootDir + 'dist/js'));
}

// ReSharper disable once IdentifierTypo
function _buildSCSS() {
    // ReSharper disable once StringLiteralTypo
    src(rootDir + "src/css/**/*.scss").pipe(sass()).pipe(dest(rootDir + "src/css"));
    // ReSharper disable once StringLiteralTypo
    return src(rootDir + "src/css/**/*.scss").pipe(sass()).pipe(dest(rootDir + "dist/css"));
}

// ReSharper disable once StringLiteralTypo
gulp.task("buildSCSS", () => _buildSCSS());

gulp.task("minify-css",
    () => src(rootDir + "dist/css/**/*.css").pipe(cleanCSS({ compatibility: "ie8" })).pipe(dest(rootDir + "dist/css"))
);

gulp.task("auto-prefix", function (done) {
    src(rootDir + "dist/css/**/*.css").pipe(autoprefixer({ browsers: ["last 2 versions"], cascade: false })).pipe(dest(rootDir + "dist/css"));
    src(rootDir + "src/css/**/*.css").pipe(autoprefixer({ browsers: ["last 2 versions"], cascade: false })).pipe(dest(rootDir + "src/css"));
    done();
});

gulp.task("read-js-files", () => _readJSFiles());

//gulp.task("default", function (done) {
//    _readJSFiles();
//    _buildSCSS();
//    _build(done);
//});

gulp.task("concat", function () {
    return src([rootDir + "src/js/Widget.js", rootDir + "src/js/SPA.js", rootDir + "src/js/*.js"]).pipe(concat("all.js")).pipe(dest(rootDir + "dist/js"));
});

gulp.task("babel", function () {
    return src(rootDir + "dist/js/all.js").pipe(babel({ presets: ["@babel/preset-env"] })).pipe(dest(rootDir + "dist/js"));
});

gulp.task("uglify", function (cb) {
    pump([
        src(rootDir + "dist/js/*.js"),
        uglify(),
        dest(rootDir + "dist/js")
    ],
        cb
    );
});

gulp.task("default",
    gulp.parallel(
        gulp.series("concat", "babel", "uglify"),
        // ReSharper disable once StringLiteralTypo
        gulp.series("buildSCSS", "auto-prefix", "minify-css")
    )
);

gulp.task("serve", function () {
    browserSync.init({ server: rootDir + "./" });
    //browserSync.init({
    //    proxy: "http://localhost:5000"
    //});
    gulp.watch(rootDir + "index.html").on("change", browserSync.reload);
    // ReSharper disable once StringLiteralTypo
    // ReSharper disable once StringLiteralTypo
    gulp.watch(rootDir + "src/css/**/*.scss", gulp.series("buildSCSS", "auto-prefix", "minify-css")).on("change", browserSync.reload);
    gulp.watch(rootDir + "src/js/**/*.js", gulp.series("concat", "babel", "uglify")).on("change", browserSync.reload);
    // ReSharper disable once StringLiteralTypo
    gulp.watch("browsersync-update.txt").on("change", browserSync.reload);
});