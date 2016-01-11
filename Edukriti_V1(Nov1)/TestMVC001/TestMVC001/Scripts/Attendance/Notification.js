
$(document).ready(function () {
    initializeTreeComponents();
    var groupManagerContainer = $(".treeContainer");
    $('#btnDisplayPhoneNumbers').on('click', function () {
        var selectedGroupIdsList = [];
        var groupsRequest = {
            selectedGroups: []
        };
        $(".plist .pill .title").each(function (index, element) {
            selectedGroupIdsList.push($(element).data("key"));
        });
        
        $.ajax({
            url: $(this).attr("data-url"),
            type: "GET",
            //contentType: 'application/json; charset=UTF-8',
            data: {
                groupIdsList: selectedGroupIdsList
            },
            traditional : true,
            //datatype: "json",
            success: function (data) {
                if (data && data.length > 0) {
                    //KNS
                    var $phoneNumbers = $("#phoneNumbers");
                    $phoneNumbers.removeData("numbers").removeAttr("numbers");
                    $phoneNumbers.val(data.join("; "));
                    $("#error").empty();
                } else {
                    $(".treeContainer").empty();
                    $("#error").empty();
                    $("<div class='text-danger alignCenter' id='error'>No records found. Please update the search criteria.</div>").insertAfter("#reportForm #buttons");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //Article.progress().hideLoading();
                $(".reportData").empty();
                $("<div class='text-danger alignCenter' id='error'>" +
                    "<p>status code: " + jqXHR.status + "</p>" +
                    "<p>errorThrown: " + errorThrown + "</p>" +
                    "<p>jqXHR.responseText: " + jqXHR.responseText + "</p></div>").insertAfter("#reportForm #buttons");
            }
        });
    });
    function initializeTreeComponents() {
        $.ajax({
            url: $("#gettreeurl").val(),
            type: "GET",
            //contentType: 'application/json; charset=UTF-8',
            datatype: "json",
            success: function (data) {
                if (data && data.length > 0) {
                    renderGroupsTree(data);
                    $("#error").empty();
                } else {
                    $(".treeContainer").empty();
                    $("#error").empty();
                    $("<div class='text-danger alignCenter' id='error'>No records found. Please update the search criteria.</div>").insertAfter("#reportForm #buttons");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //Article.progress().hideLoading();
                $(".reportData").empty();
                $("<div class='text-danger alignCenter' id='error'>" +
                    "<p>status code: " + jqXHR.status + "</p>" +
                    "<p>errorThrown: " + errorThrown + "</p>" +
                    "<p>jqXHR.responseText: " + jqXHR.responseText + "</p></div>").insertAfter("#reportForm #buttons");
            }
        });
    };

    function renderGroupsTree(groups) {
        var treeHtml = "<div class='availableGroups'>Available Groups</div><ul class='gridData' style='display:none;'>" + renderRecursiveTree(groups) + "</ul>";
        $(".treeContainer").html(treeHtml);
        //Use the above tree as the source to render the fancy tree
        $(groupManagerContainer).fancytree({
            icons: false,
            activate: function (event, data) {
                //Close the dropdown
                if (!data.node.selected) {
                    addPills(data.node);
                }
            },

            beforeSelect: function (event, data) {
                var s;
            },

            expand: function (event, data) {
                event.stopPropagation();
                return false;
            },
            debugLevel: 0
        });
        $('.group-pills .plist').on('click', 'span.pill a', function () {
            removePills($(this));
            //open groupmanager dropdown after remove
            return false;
        });
    }

    function renderRecursiveTree(groups) {
        var treeHtml = [];
        $.each(groups, function (idx, group) {
            treeHtml[treeHtml.length] = "<li id='" + group.Id + "'>" + group.GroupName;
            if (group.ChildGroups && group.ChildGroups.length > 0) {
                treeHtml[treeHtml.length] = "<ul>" + renderRecursiveTree(group.ChildGroups) + "</ul>";
            }
        });
        return treeHtml.join('');
    }

    function addPills(node) {
        var pillsList = $('.group-pills .plist');
        var pillHtml = "<span class='pill'>" +
                                    "<span class='title' data-key='" + node.key + "'>" + node.title + "</span>" +
                                    "<a class='remove-group groupManager-icon' data-key='" + node.key + "' href='javascript:void(0);' title='Remove Group'></a>" +
                                    "</span>";
        $(pillsList).append(pillHtml);
        $('.group-action').removeClass('hide');
        //Disable the assigned groups
        node.setSelected(true);
        selectSubgroups(node);  //Disable the sub-groups of the assigned groups
    }
    
    function selectSubgroups(dataNode) {
        if (dataNode.children && dataNode.children.length > 0) {
            $.each(dataNode.children, function (idx, node) {
                $("a[data-key='" + node.key + "']").trigger('click');
                node.setSelected(true);
                selectSubgroups(node);
            });
        }
    }

    function removePills(element) {
        var self = this;
        var removeItem = element.siblings('span.title').data('key') + "";
        element.parent().remove();
        var groupManagerTree = $(groupManagerContainer).fancytree('getTree');
        var removedNode = groupManagerTree.getNodeByKey(removeItem);
        removedNode.toggleSelected();
        removedNode.setActive(false);
        $(removedNode.span).removeClass('fancytree-active');
        unSelectSubgroups(removedNode);
        if ($(".pill").length === 0) {
            $('.group-action').addClass('hide');
        }
    }

    function unSelectSubgroups(dataNode) {
        if (dataNode.children && dataNode.children.length > 0) {
            $.each(dataNode.children, function (idx, node) {
                node.toggleSelected(true);
                node.setActive(false);
                unSelectSubgroups(node);
            });
        }
    }
});


/*if (typeof (Notification) == 'undefined')
    Notification = {};
Notification.renderJsonTree = function (groups) {
    var self = this;
    var treeHtml = [];
    $.each(groups, function (idx, group) {
        treeHtml[treeHtml.length] = "<li id='" + group.Id + "'>" + group.Name;
        if (group.ChildGroups && group.ChildGroups.length > 0) {
            treeHtml[treeHtml.length] = "<ul>" + self._renderJsonTree(groups, group.ChildGroups) + "</ul>";
        }
    });
    return treeHtml.join('');
};*/