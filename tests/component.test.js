//let chai = import('../node_modules/chai/chai');
// let chai = require('chai);
import { expect, should } from 'chai';
import { JSDOM } from 'jsdom';
import * as fs from 'fs';
import * as path from 'path';



describe("sample test",function () {
    before(function (done) {

        done();
    });
    //assert only
    it("should run", function (done) {

        // dynamic import
        import('chai').then(module => {
            console.log('abc');
            module.expect("<div>abc</div>").to.include("ad");
        }).catch(error => { console.log(error) });

        expect("<div>abc</div>").to.include("abc");
        done();
    });
});

describe("load static view component", function () {
    before(function (done) {
        setupDOM(done);
    })
})