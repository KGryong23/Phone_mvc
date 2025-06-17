// Admin Common Functions for Phone Management

// Global notification function using Bootstrap Toast
function showNotification(type, message) {
    const toastContainer = document.getElementById("toast-container")
    if (!toastContainer) {
        console.error("Toast container not found")
        alert(message) // Fallback
        return
    }

    const toastId = "toast-" + Date.now()
    const bgClass =
        type === "success" ? "bg-success" : type === "error" ? "bg-danger" : type === "warning" ? "bg-warning" : "bg-info"

    const toastHtml = `
        <div id="${toastId}" class="toast align-items-center text-white ${bgClass} border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    <i class="fas ${type === "success" ? "fa-check-circle" : type === "error" ? "fa-exclamation-circle" : "fa-info-circle"} me-2"></i>
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        </div>
    `

    toastContainer.insertAdjacentHTML("beforeend", toastHtml)
    const toastElement = document.getElementById(toastId)

    if (typeof bootstrap !== "undefined" && bootstrap.Toast) {
        const toast = new bootstrap.Toast(toastElement, {
            autohide: true,
            delay: 5000,
        })
        toast.show()

        // Remove toast element after it's hidden
        toastElement.addEventListener("hidden.bs.toast", () => {
            toastElement.remove()
        })
    } else {
        // Fallback if Bootstrap is not available
        alert(message)
        toastElement.remove()
    }
}

// Confirmation dialog using Bootstrap Modal
function showNotificationWithConfirm(message, confirmCallback, cancelCallback) {
    const modalId = "confirm-modal-" + Date.now()
    const modalHtml = `
        <div class="modal fade" id="${modalId}" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <i class="fas fa-question-circle text-warning me-2"></i>
                            Xác nhận
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        ${message}
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                            <i class="fas fa-times me-1"></i>Hủy
                        </button>
                        <button type="button" class="btn btn-danger" id="confirm-btn-${modalId}">
                            <i class="fas fa-check me-1"></i>Xác nhận
                        </button>
                    </div>
                </div>
            </div>
        </div>
    `

    document.body.insertAdjacentHTML("beforeend", modalHtml)
    const modalElement = document.getElementById(modalId)

    if (typeof bootstrap !== "undefined" && bootstrap.Modal) {
        const modal = new bootstrap.Modal(modalElement)

        // Handle confirm button
        document.getElementById(`confirm-btn-${modalId}`).addEventListener("click", () => {
            modal.hide()
            if (confirmCallback) confirmCallback()
        })

        // Handle modal hidden event
        modalElement.addEventListener("hidden.bs.modal", () => {
            modalElement.remove()
        })

        // Handle cancel (when modal is dismissed without confirm)
        let isConfirmed = false
        document.getElementById(`confirm-btn-${modalId}`).addEventListener("click", () => {
            isConfirmed = true
        })

        modalElement.addEventListener("hidden.bs.modal", () => {
            if (!isConfirmed && cancelCallback) {
                cancelCallback()
            }
        })

        modal.show()
    } else {
        // Fallback to browser confirm
        if (confirm(message)) {
            if (confirmCallback) confirmCallback()
        } else {
            if (cancelCallback) cancelCallback()
        }
        modalElement.remove()
    }
}

// Loading state for button containers
function toggleButtonContainerLoading(id) {
    const container = document.querySelector(`.button-container-${id}`)
    if (container) {
        const isLoading = container.classList.contains("loading")

        if (isLoading) {
            container.classList.remove("loading")
            container.style.opacity = "1"
            container.style.pointerEvents = "auto"
        } else {
            container.classList.add("loading")
            container.style.opacity = "0.6"
            container.style.pointerEvents = "none"

            // Add spinner if not exists
            if (!container.querySelector(".spinner-border")) {
                const spinner = document.createElement("div")
                spinner.className = "spinner-border spinner-border-sm ms-2"
                spinner.setAttribute("role", "status")
                container.appendChild(spinner)
            }
        }
    }
}

// Modal helper for viewing details
function openModalWithApi(url, size = "md") {
    const modalId = "api-modal-" + Date.now()
    const modalHtml = `
        <div class="modal fade" id="${modalId}" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-${size}">
                <div class="modal-content">
                    <div class="modal-body text-center p-4">
                        <div class="spinner-border text-primary" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                        <p class="mt-3 mb-0">Đang tải dữ liệu...</p>
                    </div>
                </div>
            </div>
        </div>
    `

    document.body.insertAdjacentHTML("beforeend", modalHtml)
    const modalElement = document.getElementById(modalId)

    if (typeof bootstrap !== "undefined" && bootstrap.Modal) {
        const modal = new bootstrap.Modal(modalElement)

        // Load content via fetch
        fetch(url)
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Network response was not ok")
                }
                return response.text()
            })
            .then((html) => {
                modalElement.querySelector(".modal-content").innerHTML = html
            })
            .catch((error) => {
                console.error("Error loading modal content:", error)
                modalElement.querySelector(".modal-content").innerHTML = `
                    <div class="modal-header">
                        <h5 class="modal-title text-danger">
                            <i class="fas fa-exclamation-triangle me-2"></i>Lỗi
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="alert alert-danger">
                            <i class="fas fa-exclamation-circle me-2"></i>
                            Có lỗi xảy ra khi tải dữ liệu. Vui lòng thử lại sau.
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                    </div>
                `
            })

        // Remove modal when hidden
        modalElement.addEventListener("hidden.bs.modal", () => {
            modalElement.remove()
        })

        modal.show()
    } else {
        // Fallback - open in new window
        window.open(url, "_blank")
        modalElement.remove()
    }
}

// jQuery extensions (if jQuery is available)
if (typeof jQuery !== "undefined") {
    // Custom registerGrid function for DataTables
    //jQuery.fn.registerGrid = function (options) {
    //    return this.DataTable({
    //        processing: true,
    //        serverSide: true,
    //        ajax: {
    //            url: options.endpoint,
    //            type: "POST",
    //            data: (d) => {
    //                // Ánh xạ tham số DataTable sang BaseQuery
    //                const query = {
    //                    keyword: d.search?.value || "",
    //                    skip: d.start,
    //                    take: d.length,
    //                    draw: d.draw,
    //                    // Xử lý sắp xếp
    //                    sortField: d.columns[d.order[0]?.column]?.data || "Id", // Lấy tên trường từ cột được sắp xếp
    //                    sortDirection: d.order[0]?.dir || "desc" // Lấy hướng sắp xếp (asc/desc)
    //                };
    //                if (options.prepareRequest) {
    //                    return options.prepareRequest(query);
    //                }
    //                console.log("Sending query:", query);
    //                return query;
    //            },
    //            dataSrc: (response) => {
    //                if (response.success && response.data) {
    //                    return response.data.Data || [];
    //                }
    //                showNotification("error", response.message || "Có lỗi xảy ra khi tải dữ liệu");
    //                return [];
    //            },
    //            error: (xhr, error, thrown) => {
    //                showNotification("error", "Lỗi khi tải dữ liệu: " + (xhr.responseJSON?.message || thrown));
    //                return [];
    //            }
    //        },
    //        columns: options.columns,
    //        pageLength: options.length || 10,
    //        language: {
    //            processing: "Đang xử lý...",
    //            search: "Tìm kiếm:",
    //            lengthMenu: "Hiển thị _MENU_ mục",
    //            info: "",
    //            infoEmpty: "",
    //            infoFiltered: "",
    //            loadingRecords: "Đang tải...",
    //            zeroRecords: "Không tìm thấy dữ liệu",
    //            emptyTable: "Không có dữ liệu",
    //            paginate: {
    //                first: "Đầu",
    //                previous: "Trước",
    //                next: "Sau",
    //                last: "Cuối",
    //            },
    //        },
    //        responsive: true,
    //        lengthChange: false,
    //        info: false,
    //        dom: '<"row"<"col-sm-12"t>>' +
    //            '<"row datatable-footer mt-3"<"col-sm-12 text-center"p>>'
    //    });
    //};

    // Custom registerButton function
    jQuery.fn.registerButton = function (options) {
        return this.each(function () {
            const $btn = $(this)

            // Add icon if specified
            if (options.icon) {
                const iconHtml = `<i class="${options.icon} me-2"></i>`
                if ($btn.find("i").length === 0) {
                    $btn.prepend(iconHtml)
                }
            }

            // Add click handler
            if (options.clickHandler) {
                $btn.off("click.registerButton").on("click.registerButton", (e) => {
                    e.preventDefault()

                    // Add loading state
                    const originalHtml = $btn.html()
                    $btn.prop("disabled", true).html('<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...')

                    // Execute handler
                    Promise.resolve(options.clickHandler()).finally(() => {
                        $btn.prop("disabled", false).html(originalHtml)
                    })
                })
            }
        })
    }
}

$(document).ready(() => {
    console.log("Admin common functions loaded successfully");
    if (!document.getElementById("admin-common-styles")) {
        const style = document.createElement("style");
        style.id = "admin-common-styles";
        style.textContent = `
            .button-container-in-grid a {
                display: inline-block;
                margin: 0 2px;
                padding: 6px 10px;
                border-radius: 4px;
                text-decoration: none;
                transition: all 0.2s;
            }
            .button-container-in-grid a:hover {
                background-color: rgba(0,0,0,0.1);
            }
            .button-container-in-grid.loading {
                position: relative;
            }
            .button-container-in-grid .spinner-border-sm {
                width: 1rem;
                height: 1rem;
            }
            .datatable-footer {
                padding: 15px 10px; /* Padding cho phân trang */
                background-color: #f8f9fa;
                border-top: 1px solid #dee2e6;
            }
            .dataTables_paginate {
                display: block !important;
                visibility: visible !important;
                opacity: 1 !important;
                margin: 0 !important;
            }
            .dataTables_paginate .pagination {
                justify-content: center;
                margin: 0;
            }
            .dataTables_paginate .pagination .page-item {
                margin: 0 2px;
            }
            /* Ẩn length và info nếu bị hiển thị do CSS khác */
            .dataTables_length, .dataTables_info {
                display: none !important;
            }
        `;
        document.head.appendChild(style);
    }
});