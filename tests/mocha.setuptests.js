import { JSDOM } from 'jsdom'
import * as fs from 'fs';
import * as path from 'path';

global.setupdom = function (...args) {
    let viewComponent;
    if (args.length > 1) {

    }
    global.jsDOM = new JSDOM(data = "", {
        runScripts: "dangerously", resources: "usable"
    });
    global.jsDOM.window.$ = jquery(global.jsDom.window);
}