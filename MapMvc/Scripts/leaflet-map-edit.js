contextMenuNewHandler = function(event) {
    var geoObject = event.target;
    console.log(geoObject);
    makeGeoObjectSelected(geoObject);
    window.currentGeoObjectId = geoObject.feature.properties.geoObjectId;
    if ($('#menu').css('display') == 'block') {
        $('#menu').remove();
    } else {
        var poiObject = getPoiObjectByGeoObjectId(currentGeoObjectId);
        var menuContent = '\
            <div id="menu" class="contextMenu">Тут размещён:<br />\n' + getPoiObjectsSelectList(poiObject != undefined ? poiObject.id : null) + '\
                <div>\
                    <input class="btn btn-primary" type="submit" value="Сохранить" />&nbsp;&nbsp;\
                    <input class="btn" id="btnCancel" type="button" value="Отмена" />&nbsp;&nbsp;\
                    <hr/>\
                    <input class="btn btn-mini btn-danger" id="btnRemovePerson" type="button" value="Убрать размещение" />&nbsp;&nbsp;\
                    <!-- <input class="btn btn-mini btn-danger" id="btnRemoveTable" type="button" value="Удалить объект с карты" /> --> \
                </div>\
            </div>';
        $('body').append(menuContent);
	    

        $('#menu').css({
            left:event.originalEvent.clientX,
            top:event.originalEvent.clientY
        });

        $('#menu input[type="submit"]').click(function () {
            var edit_uid = $('#edit_uid').val();
            bindPoiToObject(edit_uid, window.currentGeoObjectId);
            poiObjectsToGeoObjects[edit_uid] = window.currentGeoObjectId;
        	//Тут нужно добавить на карту попап с новым чуваком
            window.currentGeoObject.setStyle(optionsNotEmpty);
            window.currentGeoObjectStruct = {
            	geoObject: window.currentGeoObject,
            	options: $.extend({}, window.currentGeoObject.options)
            }
            var po = getPoiObjectByGeoObjectId(window.currentGeoObjectId);
            var poiObjectType = getPoiObjectTypeByGeoObjectId(window.currentGeoObjectId);
            window.currentGeoObject._popup._content = renderPoiObjectToPopup(po, poiObjectType);
            $('#menu').remove();

        });

        $('#menu #btnCancel').click(function () {
            window.currentGeoObjectId = null;
            makeGeoObjectUnselected();
            $('#menu').remove();
        });

        $('#menu #btnRemovePerson').click(function () {
            var edit_uid = $('#edit_uid').val();
            unbindPoiFromObject(edit_uid, window.currentGeoObjectId);
            poiObjectsToGeoObjects[edit_uid] = null;
            makeGeoObjectUnselected();
            window.currentGeoObject.setStyle(optionsEmpty);
            window.currentGeoObjectStruct = {
                geoObject: window.currentGeoObject,
                options: $.extend({}, window.currentGeoObject.options)
            }
            window.currentGeoObject._popup._content = "Объект №" + geoObject.feature.properties.geoObjectId + "<br/>Продаем рекламу в popup'ах. Обращайтесь";
            $('#menu').remove();
            setHash(null);
        });

        $('#menu #btnRemoveTable').click(function () {
        	//Delete Object
            makeGeoObjectUnselected();            
            $('#menu').remove();
        });
    }
}


function bindPoiToObject(poiId, objectId) {
	if (poiId == "" || poiId == undefined || objectId == undefined || objectId < 0)
		return;

	//Проверяем не сидит ли этот человек где то еще
	//TODO 

	//Проверяем еще раз, не сидит ли кто за этим столом
	var u = getPoiObjectByGeoObjectId(objectId);
	if (u != null) {
		if (!confirm("За этим столом уже сидит\n" + u.name + "\nВсе равно записать?\n(Старый сотрудник потеряет место)"))
			return;
		else
			unbindPoiFromObject(u.id);
	}

	console.log('BindPoi ' + '{"poiId":"' + poiId + '", "objectId":' + objectId + '}');
	jQuery.ajax({
		type: "POST",
		url: serviceHost + "edit/bind",
		data: '{poiId:"' + poiId + '", objectId:"' + objectId + '"}',
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		crossDomain: true,
		success: function (response) {

		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			if (XMLHttpRequest.status != 200)
				alert("Ошибка при размещении.");
		}

	});
}

function unbindPoiFromObject(poiId, objectId) {
	if (poiId == "" || poiId == undefined)
		return;

	console.log('UnbindPoi ' + '{"poiId":"' + poiId + '", "objectId":' + objectId + '}');
	jQuery.ajax({
		type: "POST",
		url: serviceHost + "edit/unbind",
		data: '{poiId:"' + poiId + '"}',
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		crossDomain: true,
		success: function (response) {

		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			if (XMLHttpRequest.status != 200)
				alert("Ошибка при размещении.");
		}

	});
}

function getPoiObjectsSelectList(currentPoiId) {
    var selectHtml = '<select id="edit_uid">\n<option value=""></option>\n';
    var optGrp1 = '<optgroup label="Не размещены на карте" style="font-style:normal; font-weight:normal;">';
    var optGrp2 = '<optgroup label="Размещены" style="font-style:normal; font-weight:normal; color: #999">';

    for (var poiObjectType in mapPOIs){
        var poiObjects = mapPOIs[poiObjectType].objects;
        for (var poiObjectKey in poiObjects){
            var poiObject = poiObjects[poiObjectKey];
            var selected = ((currentPoiId != undefined) && (currentPoiId == poiObject.id)) ? ' selected="selected"' : '';
            var hasGeoData = getGeoIdByPoiId(poiObject.id);
            var html = '<option value="' + poiObject.id + '"'
                + selected + '>'
                + poiObject.name + '</option>\n';
            if (hasGeoData != undefined && hasGeoData != 0)
                optGrp2 += html;
            else
                optGrp1 += html;
        }
    }

    optGrp1 += '</optgroup>';
    optGrp2 += '</optgroup>';
    selectHtml += optGrp1 + optGrp2 + '</select>\n';

    return selectHtml;
}