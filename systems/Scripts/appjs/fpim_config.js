/*
Copyright (c) 2003-2009, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	config.language = 'en';
	config.pasteFromWordRemoveStyle = true;
	config.protectedSource.push( /<%[\s\S]*?%>/g );   // ASP Code
	config.pasteFromWordKeepsStructure = true;
	config.resize_enabled = false;
	config.uiColor = '#dddddd';
	config.skin = 'kama';
	config.toolbarCanCollapse = false;
	config.ForcePasteAsPlainText = true;
	config.AutoDetectPasteFromWord = true;
	config.toolbar_Basic = [['Cut','Copy','Paste','PasteFromWord','Undo','Redo','Bold','Italic','Source']] ;
/*
	config.toolbar_Full = [['Preview'],
	['Cut','Copy','Paste','PasteText','PasteFromWord','-','SpellCheck'],
	['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
	['Bold','Italic','Underline','StrikeThrough','-','Subscript','Superscript'],
	['NumberedList','BulletedList'],
	['Link','Unlink','Anchor','Image','Source']] ;
*/

	config.toolbar_Full =
	[
		{ name: 'clipboard', items : [ 'Cut','Copy','Paste','PasteText','PasteFromWord','Undo','Redo' ] },
		{ name: 'editing', items : [ 'Find','Replace','-','SelectAll','-','SpellChecker' ] },
		{ name: 'basicstyles', items : [ 'Bold','Italic','Strike','Subscript','Superscript','-','RemoveFormat' ] },
		{ name: 'paragraph', items : [ 'NumberedList','BulletedList','-','Outdent','Indent' ] },
		{ name: 'links', items : [ 'Link','Unlink','Anchor' ] },
		{ name: 'insert', items : [ 'Image','Table','HorizontalRule','SpecialChar','PageBreak','Iframe' ] },
		{ name: 'source', items : [ 'Source' ] }
	];

};
