var bupa = function () {

    var _showMask = function (block, msg) {
        if (block) {
            $(block).block({
                message: "<img src='/img/loader.png' class='img-fluid loader-img' /><span class='loader-text'>" + (msg != undefined ? msg : "Lütfen bekleyiniz...") + "</span>",
                overlayCSS: {
                    backgroundColor: "#fff",
                    opacity: 0.8,
                    cursor: "wait",
                    "box-shadow": "0 0 0 1px #ddd"
                },
                css: {
                    border: 0,
                    padding: 0,
                    backgroundColor: "none"
                }
            });
        }
        else {
            $.blockUI({
                message: "<img src='/img/loader.png' class='img-fluid loader-img' /><span class='loader-text'>" + (msg != undefined ? msg : "Lütfen bekleyiniz...") + "</span>",
                overlayCSS: {
                    backgroundColor: "#fff",
                    opacity: 0.8,
                    cursor: "wait",
                    "box-shadow": "0 0 0 1px #ddd"
                },
                css: {
                    border: 0,
                    padding: 0,
                    backgroundColor: "none"
                }
            });
        }
    }

    var _hideMask = function (block) {
        if (block) {
            $(block).unblock();
        }
        else {
            $.unblockUI();
        }
    }

    var _endsWith = function (str, suffix) {
        if (!str.length) {
            return false;
        }
        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    }

    var _showModal = function (title, body, fn) {
        var zindex = $(".modal:last").css("z-index");

        var modal = $("<div>", { class: "modal fade", tabindex: "-1" }).append(
            $("<div>", { class: "modal-dialog" }).append(
                $("<div>", { class: "modal-content" }).append(
                    $("<div>", { class: "modal-header " }).append(
                        $("<h5>", { class: "modal-title", html: title }),
                        $("<button>", { type: "button", class: "close", "data-dismiss": "modal", html: '<span aria-hidden="true">&times;</span>' }),
                    ),
                    $("<div>", { class: "modal-body" }).append(
                        body
                    ),
                    $("<div>", { class: "modal-footer" }).append(
                        $("<button>", { type: "button", class: "btn btn-link", "data-dismiss": "modal", html: "Kapat" }),
                        $("<button>", { type: "button", class: "btn ok-btn ", html: "Kaydet" })
                    )
                )
            )
        );
        if (!title) {
            modal.find(".modal-title").remove();
        }
        if (!fn) {
            modal.find(".modal-footer").remove();
        }

        modal.appendTo("body");

        if (fn && typeof fn == "function") {
            modal.find(".ok-btn").click(fn);
        }

        modal.on("hidden.bs.modal", function (e) {
            if (e.target === this) {
                modal.remove();
                if ($(".modal.show").length) {
                    $("body").addClass("modal-open");
                }
            }
        });
        modal.modal({ backdrop: 'static' });
        if (zindex) {
            var z = parseInt(zindex);
            $(".modal-backdrop:last").css("z-index", z + 1);
            modal.css("z-index", z + 2);
        }

        return modal;
    }

    var _alert = function (type, msg) {
        if (type == "danger") {
            msg = !msg || msg == "" ? "Beklenmedik bir hata oluştu" : msg;
            msg = '<i class="fas fa-exclamation-circle text-danger"></i> ' + msg;
        }
        else {
            msg = !msg || msg == "" ? "İşlem başarılı" : msg;
            msg = '<i class="fas fa-check-circle text-success"></i> ' + msg;
        }
        return _showModal(undefined, msg, undefined);
    }

    var _ajaxPost = function (url, mask, data, done, fail) {
        _showMask(mask);

        var jx = $.ajax({
            type: "POST",
            url: url,
            data: data,
            cache: false,
            success: function (result) {
                _hideMask(mask);
                if (done && typeof done === "function") {
                    done.call(undefined, result);
                }
            },
            error: function (xhr, status, error) {
                _hideMask(mask);
                if (fail && typeof fail === "function") {
                    fail.call(undefined);
                }
            }
        });
        return jx;
    }

    var _ajaxGet = function (url, mask, data, done, fail) {
        _showMask(mask);

        var jx = $.ajax({
            type: 'GET',
            url: url,
            data: data,
            cache: false,
            success: function (result) {
                _hideMask(mask);

                if (result.redirect) {
                    _goto(result.redirect);
                }

                if (result && typeof result === 'object') {
                    if (result.hasError !== undefined) {
                        if (result.hasError) {
                            if (result.errors && result.errors.length) {
                                _showModal("danger", "Hata", _resultError(result));
                            }
                            return;
                        }
                    }
                }

                if (done && typeof done === 'function') {
                    done.call(undefined, result);
                }
            },
            error: function (xhr, status, error) {
                _hideMask(mask);
                if (fail && typeof fail === 'function') {
                    fail.call(undefined);
                }
                else {
                    _showModal("danger", "Hata", "Beklenmedik bir hata oluştu");
                }
            }
        });

        return jx;
    }

    var _goto = function (url) {
        _showMask("body", "Yönlendiriliyorsunuz. Lütfen bekleyiniz...");
        window.location.href = url;
    }

    var _resultError = function (result) {
        var msg = [];
        for (i = result.errors.length - 1; i >= 0; i--) {
            msg.push(result.errors[i].message);
        }
        return msg.join("<br>");
    }

    var _getHref = function () {
        return window.location.pathname + window.location.search + window.location.hash;
    }

    var _changeUrlParam = function (name, val, url) {
        if (!url) {
            url = _getHref();
        }
        if (url.indexOf(name + '=') >= 0) {
            var prefix = url.substring(0, url.indexOf(name));
            var suffix = url.substring(url.indexOf(name));
            suffix = suffix.substring(suffix.indexOf("=") + 1);
            suffix = (suffix.indexOf('&') >= 0) ? suffix.substring(suffix.indexOf('&')) : '';
            return prefix + name + '=' + encodeURIComponent(val) + suffix;
        }
        else {
            if (url.indexOf("?") < 0) {
                return url + '?' + name + '=' + encodeURIComponent(val);
            }
            else {
                return url + '&' + name + '=' + encodeURIComponent(val);
            }
        }
    }

    var _getUrlParam = function (name, url) {
        if (!url) {
            url = _getHref();
        }
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(url);
        return results ? results[1] : null;
    }

    var _numericInput = function (element) {
        $(element).keydown(function (e) {
            if (-1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || (/65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey)) || (35 <= e.keyCode && 40 >= e.keyCode)) {
                return;
            }

            if ($(this).is("[type='number'][maxlength]") && this.value.length > this.maxLength - 1) {
                e.preventDefault();
                return;
            }

            var keyCode = e.keyCode || e.which;
            if (keyCode >= 96 && keyCode <= 105) {
                keyCode -= 48;
            }

            var valid = /^[0-9]+$/.test(String.fromCharCode(keyCode));
            if (!valid) {
                e.preventDefault();
            }
        }).on("keyup blur", function (e) {
            $(this).val($(this).val().replace(/[^0-9]/g, ''));
        });
    }

    var _formatMoney = function (input, precisionScale) {
        var floatInput = parseFloat(input);
        if (isNaN(floatInput)) {
            return "";
        }
        if (!precisionScale) {
            precisionScale = 2
        }

        var decimalPart = floatInput.toString().substr(floatInput.toString().indexOf(".") + 1, precisionScale);
        var toFixed = floatInput.toString().substr(0, floatInput.toString().indexOf("."));
        if (floatInput.toString().indexOf(".") == -1) {
            decimalPart = "";
            toFixed = floatInput.toString();
        }
        var result = toFixed.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");

        result += "," + decimalPart;
        if (result.indexOf(",") == -1) {
            result += ",";

            if (precisionScale) {
                for (var i = 0; i < precisionScale; i++) {
                    result += "0";
                }
            }
            else {
                result += "00";
            }
        }
        else {
            var currentScale = result.substr(result.indexOf(",") + 1).length;
            for (var i = currentScale; i < precisionScale; i++) {
                result += "0";
            }

        }
        return result;
    }

    var _formatDate = function (date, hour) {
        var formated = _pad(date.getDate(), 2) + "/" + _pad((date.getMonth() + 1), 2) + "/" + date.getFullYear();
        if (hour) {
            formated += " " + _pad(date.getHours(), 2) + ":" + _pad(date.getMinutes(), 2) + ":" + _pad(date.getSeconds(), 2);
        }
        return formated;
    }

    var _pad = function (str, width, chr) {
        chr = chr || '0';
        str = str + '';
        return str.length >= width ? str : new Array(width - str.length + 1).join(chr) + str;
    }

    var _prepareSubmit = function (form, fn) {
        $(form).submit(function (event) {
            var $form = $(this);
            var callFn = fn && typeof fn === "function";
            if (callFn || !$form.is("[data-submit='true']")) {
                event.preventDefault();
            }
            if (window["grecaptcha"]) {
                var siteKey = $("#site_key").val();
                grecaptcha.ready(function () {
                    grecaptcha.execute(siteKey, { action: "submit" }).then(function (token) {
                        var input = $form.find(":input[name='token']");
                        if (!input.length) {
                            input = $("<input>", { type: "hidden", name: "token" });
                            $form.append(input);
                        }
                        input.val(token);

                        if (fn && typeof fn === "function") {
                            if ($form.valid()) {
                                fn.call($form);
                            }
                        }
                        else if (!$form.is("[data-submit='true']")) {
                            $form.attr("data-submit", "true");
                            setTimeout(function () {
                                $form.submit();
                                $form.removeAttr("data-submit");
                            }, 200);
                        }
                    });
                });
            }
            else {
                if (fn && typeof fn === "function") {
                    if ($form.valid()) {
                        fn.call($form);
                    }
                }
                else if (!$form.is("[data-submit='true']")) {
                    $form.attr("data-submit", "true");
                    setTimeout(function () {
                        $form.submit();
                        $form.removeAttr("data-submit");
                    }, 200);
                }
            }

        });
    }
    var _setCookie = function () {
        $.cookie("_ckplcm", "true", { expires: 365, path: '/' });
    }
    return {
        showMask: function (block, msg) {
            return _showMask(block, msg);
        },
        hideMask: function (block) {
            return _hideMask(block);
        },
        showModal: function (title, body, fn) {
            return _showModal(title, body, fn);
        },
        alert: function (type, msg) {
            return _alert(type, msg);
        },
        ajaxPost: function (url, mask, data, done, fail) {
            return _ajaxPost(url, mask, data, done, fail);
        },
        ajaxGet: function (url, mask, data, done, fail) {
            return _ajaxGet(url, mask, data, done, fail);
        },
        resultError: function (result) {
            return _resultError(result);
        },
        changeUrlParam: function (name, val, url) {
            return _changeUrlParam(name, val, url);
        },
        getUrlParam: function (name, url) {
            return _getUrlParam(name, url);
        },
        numericInput: function (element) {
            _numericInput(element);
        },
        formatMoney: function (input, precisionScale) {
            return _formatMoney(input, precisionScale);
        },
        formatDate: function (date, hour) {
            return _formatDate(date, hour);
        },
        prepareSubmit: function (form, fn) {
            return _prepareSubmit(form, fn);
        },
        goto: function (url) {
            return _goto(url);
        },
        setCookie: function (form, fn) {
            return _setCookie(form, fn);
        }
    };
}();