/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
    // Define the toolbar groups as it is a more accessible solution.
    config.toolbarGroups = [
        {"name":"basicstyles","groups":["basicstyles"]},
        {"name":"links","groups":["links"]},
        {"name":"paragraph","groups":["list","blocks"]},
        {"name":"document","groups":["mode"]},
        {"name":"insert","groups":["insert"]},
        {"name":"styles","groups":["styles"]},
        {"name":"about","groups":["about"]}
    ];
    config.removeButtons = 'Underline,Strike,Subscript,Superscript,Anchor,Styles,Specialchar';
    config.toolbarCanCollapse = true;
    config.toolbarStartupExpanded = false;
};
