const gulp = require('gulp');
const {src, dest} = require('gulp');
const browserSync = require('browser-sync').create();
const sass = require('gulp-sass');
const order = require('gulp-order');
const babel = require('gulp-babel');
const uglify = require('gulp-uglify');
const cleanCSS = require('gulp-clean-css');
const autoprefixer = require('gulp-autoprefixer');
const concat = require('gulp-concat');
const pump = require('pump');

sass.compiler = require('node-sass');

gulp.task('build', done => {
    _build(done)
});

function _build(done) {
    console.log('Running build task task');
    done()
}

function _readJSFiles(){
    return src("src/js/**/*.js").pipe(dest('dist/js'))
}

function _buildSCSS() {
    src("src/css/**/*.scss").pipe(sass()).pipe(dest('src/css'));
    return src("src/css/**/*.scss").pipe(sass()).pipe(dest('dist/css'))
}

gulp.task('buildSCSS', () => _buildSCSS());

gulp.task('minify-css',
    () => src("dist/css/**/*.css").pipe(cleanCSS({compatibility: 'ie8'})).pipe(dest('dist/css'))
);

gulp.task('autoprefix', function (done) {
    src('dist/css/**/*.css').pipe(autoprefixer({browsers: ['last 2 versions'], cascade: false})).pipe(dest('dist/css'));
    src('src/css/**/*.css').pipe(autoprefixer({browsers: ['last 2 versions'], cascade: false})).pipe(dest('src/css'));
    done()
});

gulp.task('read-js-files', () => _readJSFiles());

gulp.task('default', function (done) {
    _readJSFiles();
    _buildSCSS();
    _build(done)
});

gulp.task('concat', function () {
    return src("src/js/**/*.js").pipe(concat('all.js')).pipe(dest('dist/js'))
});

gulp.task('babel',function () {
    return src("dist/js/all.js").pipe(babel({presets: ['@babel/preset-env']})).pipe(dest('dist/js'))
});

gulp.task('uglify',function (cb) {
    pump([
            src('dist/js/*.js'),
            uglify(),
            dest('dist/js')
        ],
        cb
    );
});

gulp.task('serve',function () {
    browserSync.init({server: "./"});
    gulp.watch("index.html").on('change', browserSync.reload);
    gulp.watch("src/css/**/*.scss",gulp.series('buildSCSS','autoprefix','minify-css')).on('change',browserSync.reload);
    gulp.watch("src/js/**/*.js",gulp.series('concat','babel','uglify')).on('change',browserSync.reload);
});