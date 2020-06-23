/*
 * poiyee.ho
 */
(function($){

    var keyboardLayout = {
        'layout': [
            // alphanumeric keyboard type
            // text displayed on keyboard button, keyboard value, keycode, column span, new row
            [
                [
                    
                    ['₁', '₁', 192, 0, true], ['₂', '₂', 49, 0, false], ['₃', '₃', 50, 0, false], ['₄', '₄', 51, 0, false], ['₅', '₅', 52, 0, false], ['₆', '₆', 53, 0, false], ['₇', '₇', 54, 0, false], ['₈', '₈', 54, 0, false], ['₉', '₉', 54, 0, false], ['₀', '₀', 54, 0, false],
                    ['¹', '¹', 49, 0, true], ['²', '²', 50, 0, false], ['³', '³', 51, 0, false], ['⁴', '⁴', 52, 0, false], ['⁵', '⁵', 53, 0, false], ['⁶', '⁶', 54, 0, false], ['⁷', '⁷', 55, 0, false], ['⁸', '⁸', 56, 0, false], ['⁹', '⁹', 57, 0, false], ['⁰', '⁰', 48, 0, false],

                    ['≤', '≤', 189, 0, true], ['≥', '≥', 187, 0, false], ['<', '<', 81, 0, false], ['>', '>', 87, 0, false], ['±', '±', 69, 0, false], ['¼', '¼', 82, 0, false], ['½', '½', 84, 0, false], ['¾', '¾', 89, 0, false], ['‖', '‖', 85, 0, false], ['∞', '∞', 74, 0, false],
                    ['μ', 'μ', 73, 0, true], ['ϕ', 'ϕ', 79, 0, false], ['ω', 'ω', 80, 0, false], ['α', 'α', 219, 0, false], ['β', 'β', 221, 0, false], ['γ', 'γ', 220, 0, false], ['√', '√', 65, 0, false], ['∛', '∛', 83, 0, false], ['∜', '∜', 68, 0, false], ['∑', '∑', 72, 0, false],
                    ['‰', '‰', 70, 0, true], ['‱', '‱', 71, 0, false],['ⁿ', 'ⁿ', 75, 0, false], 

                    
                    ['←', '8', 8, 3, false], ['Temizle', '46', 46, 3, false], ['Kapat', '13', 13, 5, false]
                ]
            ]
        ]
    };

    var activeInput = {
        'htmlElem': '',
        'initValue': '',
        'keyboardLayout': keyboardLayout,
        'keyboardType': '0',
        'keyboardSet': 0,
        'dataType': 'string',
        'isMoney': false,
        'thousandsSep': ',',
        'disableKeyboardKey': false
    };

    /*
     * initialize keyboard
     * @param {type} settings
     */
    $.fn.initKeypad = function(settings){
        //$.extend(activeInput, settings);

        $(this).click(function(e){
            activateKeypad(e.target);

        });
    };
    
    /*
     * create keyboard container and keyboard button
     * @param {DOM object} targetInput
     */
    function activateKeypad(targetInput){
        if($('div.jQKeyboardContainer').length === 0)
        {
            activeInput.htmlElem = $(targetInput);
            activeInput.initValue = $(targetInput).val();

            $(activeInput.htmlElem).addClass('focus');
            createKeypadContainer();
            createKeypad(0);
            $('div.jQKeyboardContainer').draggable();
        }
    }
    
    /*
     * create keyboard container
     */
    function createKeypadContainer(){
        var container = document.createElement('div');
        container.setAttribute('class', 'jQKeyboardContainer draggable');
        container.setAttribute('id', 'jQKeyboardContainer');
        container.setAttribute('name', 'keyboardContainer' + activeInput.keyboardType);
        
        $('body').append(container);
    }
    
    /*
     * create keyboard
     * @param {Number} set
     */
    function createKeypad(set){
        $('#jQKeyboardContainer').empty();
        
        var layout = activeInput.keyboardLayout.layout[activeInput.keyboardType][set];

        for(var i = 0; i < layout.length; i++){

            if(layout[i][4]){
                var row = document.createElement('div');
                row.setAttribute('class', 'jQKeyboardRow');
                row.setAttribute('name', 'jQKeyboardRow');
                $('#jQKeyboardContainer').append(row);
            }

            var button = document.createElement('button');
            button.setAttribute('type', 'button');
            button.setAttribute('name', 'key' + layout[i][2]);
            button.setAttribute('id', 'key' + layout[i][2]);
            button.setAttribute('class', 'jQKeyboardBtn' + ' ui-button-colspan-' + layout[i][3]);
            button.setAttribute('data-text', layout[i][0]);
            button.setAttribute('data-value', layout[i][1]);
            button.innerHTML = layout[i][0];
            
            $(button).click(function(e){
               getKeyPressedValue(e.target); 
            });

            $(row).append(button);
        }
    }
    /*
     * remove keyboard from kepad container
     */
    function removeKeypad(){
        $('#jQKeyboardContainer').remove();
        $(activeInput.htmlElem).removeClass('focus');
    }
    
    /*
     * handle key pressed
     * @param {DOM object} clickedBtn
     */
    function getKeyPressedValue(clickedBtn){
        var caretPos = getCaretPosition(activeInput.htmlElem);
        var keyCode = parseInt($(clickedBtn).attr('name').replace('key', ''));
        
        var currentValue = $(activeInput.htmlElem).val();
        var newVal = currentValue;
        var closeKeypad = false;
        
        /*
         * TODO
        if(activeInput.isMoney && activeInput.thousandsSep !== ''){
            stripMoney(currentValue, activeInput.thousandsSep);
        }
        */
        
        switch(keyCode){
            case 8:     // backspace key
                newVal = onDeleteKeyPressed(currentValue, caretPos);
		          caretPos--;
                break;
            case 13:    // enter key
                closeKeypad = onEnterKeyPressed();
                break;
            case 16:    // shift key
                onShiftKeyPressed();
                break;
            case 27:    // cancel key
                closeKeypad = true;
                newVal = onCancelKeyPressed(activeInput.initValue);
                break;
            case 32:    // space key
                newVal = onSpaceKeyPressed(currentValue, caretPos);
                caretPos++;
		break;
            case 46:    // clear key
                newVal = onClearKeyPressed();
                break;
            case 190:   // dot key
                newVal = onDotKeyPressed(currentValue, $(clickedBtn), caretPos);
                caretPos++;
                break;
            default:    // alpha or numeric key
                newVal = onAlphaNumericKeyPressed(currentValue, $(clickedBtn), caretPos);
                caretPos++;
                break;
        }
        
        // update new value and set caret position
        $(activeInput.htmlElem).val(newVal);
        setCaretPosition(activeInput.htmlElem, caretPos);

        if(closeKeypad){
            removeKeypad();
            $(activeInput.htmlElem).blur();
        }
    }
    
    /*
     * handle delete key pressed
     * @param value 
     * @param inputType
     */
    function onDeleteKeyPressed(value, caretPos){
        var result = value.split('');
        
        if(result.length > 1){
            result.splice((caretPos - 1), 1);
            return result.join('');
        }
    }
    
    /*
     * handle shift key pressed
     * update keyboard layout and shift key color according to current keyboard set
     */
    function onShiftKeyPressed(){
        var keyboardSet = activeInput.keyboardSet === 0 ? 1 : 0;
        activeInput.keyboardSet = keyboardSet;

        createKeypad(keyboardSet);
        
        if(keyboardSet === 1){
            $('button[name="key16"').addClass('shift-active');
        }else{
            $('button[name="key16"').removeClass('shift-active');
        }
    }
    
    /*
     * handle space key pressed
     * add a space to current value
     * @param {String} curVal
     * @returns {String}
     */
    function onSpaceKeyPressed(currentValue, caretPos){
        return insertValueToString(currentValue, ' ', caretPos);
    }
    
    /*
     * handle cancel key pressed
     * revert to original value and close key pad
     * @param {String} initValue
     * @returns {String}
     */
    function onCancelKeyPressed(initValue){
        return initValue;
    }
    
    /*
     * handle enter key pressed value
     * TODO: need to check min max value
     * @returns {Boolean}
     */
    function onEnterKeyPressed(){
        return true;
    }
    
    /*
     * handle clear key pressed
     * clear text field value
     * @returns {String}
     */
    function onClearKeyPressed(){
        return '';
    }
    
    /*
     * handle dot key pressed
     * @param {String} currentVal
     * @param {DOM object} keyObj
     * @returns {String}
     */
    function onDotKeyPressed(currentValue, keyElement, caretPos){
        return insertValueToString(currentValue, keyElement.attr('data-value'), caretPos);
    }
    
    /*
     * handle all alpha numeric keys pressed
     * @param {String} currentVal
     * @param {DOM object} keyObj
     * @returns {String}
     */
    function onAlphaNumericKeyPressed(currentValue, keyElement, caretPos){
        return insertValueToString(currentValue, keyElement.attr('data-value'), caretPos);
    }
    
    /*
     * insert new value to a string at specified position
     * @param {String} currentValue
     * @param {String} newValue
     * @param {Number} pos
     * @returns {String}
     */
    function insertValueToString(currentValue, newValue, pos){
        var result = currentValue.split('');
        result.splice(pos, 0, newValue);
         
        return result.join('');
    }
    
   /*
    * get caret position
    * @param {DOM object} elem
    * @return {Number}
    */
    function getCaretPosition(elem){
        var input = $(elem).get(0);

        if('selectionStart' in input){    // Standard-compliant browsers
            return input.selectionStart;
        } else if(document.selection){    // IE
            input.focus();
            
            var sel = document.selection.createRange();
            var selLen = document.selection.createRange().text.length;
            
            sel.moveStart('character', -input.value.length);
            return sel.text.length - selLen;
        }
    }
    
    /*
     * set caret position
     * @param {DOM object} elem
     * @param {Number} pos
     */
    function setCaretPosition(elem, pos){
        var input = $(elem).get(0);
        
        if(input !== null) {
            if(input.createTextRange){
                var range = elem.createTextRange();
                range.move('character', pos);
                range.select();
            }else{
                input.focus();
                input.setSelectionRange(pos, pos);
            }
        }
    }
})(jQuery);
