// MLM Generation Plan - Enhanced Dynamic JavaScript

// Global variables
let statisticsUpdateInterval;
let currentRefreshRate = 30000; // 30 seconds

// Auto-dismiss alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function() {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(function(alert) {
        setTimeout(function() {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
    
    // Initialize dynamic features based on page
    initializeDynamicFeatures();
});

// Initialize dynamic features
function initializeDynamicFeatures() {
    // Dashboard features
    if (document.querySelector('.dashboard-stats')) {
        initializeDashboard();
    }
    
    // Admin panel features
    if (document.getElementById('userTable')) {
        initializeAdminPanel();
    }
    
    // Form enhancements
    initializeFormEnhancements();
    
    // Real-time updates
    initializeRealTimeUpdates();
}

// Dashboard dynamic features
function initializeDashboard() {
    // Auto-refresh statistics
    if (document.getElementById('autoRefreshToggle')) {
        document.getElementById('autoRefreshToggle').addEventListener('change', function() {
            if (this.checked) {
                startStatisticsRefresh();
            } else {
                stopStatisticsRefresh();
            }
        });
    }
    
    // Refresh button
    const refreshBtn = document.getElementById('refreshStatsBtn');
    if (refreshBtn) {
        refreshBtn.addEventListener('click', function() {
            refreshDashboardStatistics();
        });
    }
    
    // Start auto-refresh if enabled
    const autoRefresh = document.getElementById('autoRefreshToggle');
    if (autoRefresh && autoRefresh.checked) {
        startStatisticsRefresh();
    }
}

// Refresh dashboard statistics via AJAX
function refreshDashboardStatistics() {
    const btn = document.getElementById('refreshStatsBtn');
    if (btn) {
        btn.disabled = true;
        btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Refreshing...';
    }
    
    fetch('/Dashboard/GetStatistics')
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                showNotification('Error loading statistics', 'danger');
                return;
            }
            
            // Update statistics cards
            updateStatCard('directReferrals', data.totalDirectReferrals);
            updateStatCard('totalTeamMembers', data.totalTeamMembers);
            updateStatCard('totalIncome', '₹' + parseFloat(data.totalIncome).toLocaleString('en-IN', {minimumFractionDigits: 2, maximumFractionDigits: 2}));
            
            // Update generation levels table
            updateGenerationLevelsTable(data.generationLevels);
            
            // Show success notification with SweetAlert
            Swal.fire({
                title: 'Updated!',
                text: 'Statistics refreshed successfully!',
                icon: 'success',
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });
            
            if (btn) {
                btn.disabled = false;
                btn.innerHTML = '<i class="fas fa-sync-alt"></i> Refresh';
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showNotification('Failed to refresh statistics', 'danger');
            if (btn) {
                btn.disabled = false;
                btn.innerHTML = '<i class="fas fa-sync-alt"></i> Refresh';
            }
        });
}

// Update statistics card
function updateStatCard(cardId, value) {
    const card = document.getElementById(cardId);
    if (card) {
        // Animate the number change
        const currentValue = parseInt(card.textContent.replace(/[^\d]/g, '')) || 0;
        const targetValue = parseInt(value.toString().replace(/[^\d]/g, '')) || 0;
        
        if (currentValue !== targetValue) {
            animateNumber(card, currentValue, targetValue, value.toString().includes('₹'));
        }
    }
}

// Animate number change
function animateNumber(element, from, to, isCurrency = false) {
    const duration = 500;
    const start = Date.now();
    const prefix = isCurrency ? '₹' : '';
    
    function update() {
        const now = Date.now();
        const progress = Math.min((now - start) / duration, 1);
        const current = Math.floor(from + (to - from) * progress);
        
        if (isCurrency) {
            element.textContent = prefix + current.toLocaleString('en-IN', {minimumFractionDigits: 2, maximumFractionDigits: 2});
        } else {
            element.textContent = current;
        }
        
        if (progress < 1) {
            requestAnimationFrame(update);
        } else {
            element.textContent = isCurrency ? prefix + to.toLocaleString('en-IN', {minimumFractionDigits: 2, maximumFractionDigits: 2}) : to;
        }
    }
    
    update();
}

// Update generation levels table
function updateGenerationLevelsTable(levels) {
    const tbody = document.querySelector('#generationLevelsTable tbody');
    if (!tbody || !levels) return;
    
    tbody.innerHTML = '';
    levels.forEach(level => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td><span class="badge bg-primary">Level ${level.level}</span></td>
            <td><strong>${level.memberCount}</strong></td>
            <td>₹${parseFloat(level.incomePerMember).toLocaleString('en-IN', {minimumFractionDigits: 2, maximumFractionDigits: 2})}</td>
            <td><strong class="text-success">₹${parseFloat(level.totalIncome).toLocaleString('en-IN', {minimumFractionDigits: 2, maximumFractionDigits: 2})}</strong></td>
        `;
        tbody.appendChild(row);
    });
}

// Start statistics refresh interval
function startStatisticsRefresh() {
    if (statisticsUpdateInterval) {
        clearInterval(statisticsUpdateInterval);
    }
    
    refreshDashboardStatistics();
    statisticsUpdateInterval = setInterval(refreshDashboardStatistics, currentRefreshRate);
    
    // Show indicator
    showRefreshIndicator(true);
}

// Stop statistics refresh
function stopStatisticsRefresh() {
    if (statisticsUpdateInterval) {
        clearInterval(statisticsUpdateInterval);
        statisticsUpdateInterval = null;
    }
    showRefreshIndicator(false);
}

// Show refresh indicator
function showRefreshIndicator(show) {
    let indicator = document.getElementById('refreshIndicator');
    if (!indicator) {
        indicator = document.createElement('div');
        indicator.id = 'refreshIndicator';
        indicator.className = 'position-fixed bottom-0 end-0 m-3';
        indicator.style.zIndex = '9999';
        document.body.appendChild(indicator);
    }
    
    if (show) {
        indicator.innerHTML = '<div class="alert alert-info mb-0"><i class="fas fa-sync-alt fa-spin"></i> Auto-refreshing every 30s</div>';
    } else {
        indicator.innerHTML = '';
    }
}

// Get anti-forgery token
function getAntiForgeryToken() {
    // Try multiple ways to get the token
    let tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    if (!tokenInput) {
        tokenInput = document.querySelector('form input[name="__RequestVerificationToken"]');
    }
    if (!tokenInput) {
        // Check in meta tag or create one
        tokenInput = document.querySelector('meta[name="__RequestVerificationToken"]');
    }
    return tokenInput ? tokenInput.value : '';
}

// Initialize admin panel with proper handlers
function initializeAdminPanel() {
    // Search functionality
    const searchInput = document.getElementById('userSearch');
    if (searchInput) {
        let searchTimeout;
        searchInput.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                searchUsers(this.value);
            }, 300);
        });
    }
    
    // Export functionality
    const exportBtn = document.getElementById('exportUsersBtn');
    if (exportBtn) {
        exportBtn.addEventListener('click', function() {
            Swal.fire({
                title: 'Exporting...',
                text: 'Preparing CSV file',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
            
            setTimeout(() => {
                exportUsersToCSV();
                Swal.fire({
                    title: 'Exported!',
                    text: 'Users exported successfully!',
                    icon: 'success',
                    timer: 2000,
                    showConfirmButton: false
                });
            }, 500);
        });
    }
    
    // Filter buttons
    const filterButtons = document.querySelectorAll('.filter-btn');
    filterButtons.forEach(btn => {
        btn.addEventListener('click', function() {
            const filter = this.dataset.filter;
            filterUsers(filter);
        });
    });
    
    // Attach toggle handlers to existing forms
    attachFormHandlers();
}

// Search users
function searchUsers(searchTerm) {
    const tableBody = document.querySelector('#userTable tbody');
    if (!tableBody) return;
    
    // Show loading
    tableBody.innerHTML = '<tr><td colspan="8" class="text-center"><i class="fas fa-spinner fa-spin"></i> Searching...</td></tr>';
    
    fetch(`/Admin/SearchUsers?searchTerm=${encodeURIComponent(searchTerm)}`)
        .then(response => response.json())
        .then(users => {
            renderUsersTable(users);
        })
        .catch(error => {
            console.error('Error:', error);
            tableBody.innerHTML = '<tr><td colspan="8" class="text-center text-danger">Error loading users</td></tr>';
        });
}

// Render users table
function renderUsersTable(users) {
    const tableBody = document.querySelector('#userTable tbody');
    if (!tableBody) return;
    
    if (users.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="8" class="text-center">No users found</td></tr>';
        return;
    }
    
    tableBody.innerHTML = users.map(user => `
        <tr>
            <td><strong>${user.userId}</strong></td>
            <td>${user.fullName}</td>
            <td>${user.email}</td>
            <td>${user.mobileNumber}</td>
            <td>${user.sponsorId}</td>
            <td>${user.registrationDate}</td>
            <td>
                ${user.isActive ? '<span class="badge bg-success">Active</span>' : '<span class="badge bg-danger">Inactive</span>'}
                ${user.isAdmin ? '<span class="badge bg-warning">Admin</span>' : ''}
            </td>
            <td>
                <div class="btn-group" role="group">
                    <a href="/Admin/UserDetails?userId=${user.userId}" class="btn btn-sm btn-info" title="View Details">
                        <i class="fas fa-eye"></i>
                    </a>
                    <a href="/Admin/ViewGenerationTree?userId=${user.userId}" class="btn btn-sm btn-primary" title="View Tree">
                        <i class="fas fa-sitemap"></i>
                    </a>
                    <button onclick="toggleUserStatusAjax(${user.id}, ${!user.isActive}, '${user.userId}')" class="btn btn-sm ${user.isActive ? 'btn-warning' : 'btn-success'}" title="${user.isActive ? 'Deactivate' : 'Activate'}">
                        <i class="fas ${user.isActive ? 'fa-ban' : 'fa-check'}"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
    
    // Re-attach form handlers
    attachFormHandlers();
}

// Toggle user status via AJAX (called from button onclick)
function toggleUserStatusAjax(userId, isActive, userName) {
    const actionText = isActive ? 'activate' : 'deactivate';
    
    Swal.fire({
        title: `Are you sure?`,
        html: `Do you want to <strong>${actionText}</strong> user <strong>${userName}</strong>?`,
        icon: actionText === 'activate' ? 'success' : 'warning',
        showCancelButton: true,
        confirmButtonColor: actionText === 'activate' ? '#4facfe' : '#fa709a',
        cancelButtonColor: '#6c757d',
        confirmButtonText: `Yes, ${actionText} it!`,
        cancelButtonText: 'Cancel',
        reverseButtons: true,
        customClass: {
            popup: 'animated-popup'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            performToggleUserStatus(userId, isActive);
        }
    });
}

// Perform the actual toggle
function performToggleUserStatus(userId, isActive) {
    const token = getAntiForgeryToken();
    const formData = new FormData();
    formData.append('userId', userId);
    formData.append('isActive', isActive);
    formData.append('__RequestVerificationToken', token);
    
    // Show loading
    Swal.fire({
        title: 'Processing...',
        text: 'Please wait',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    fetch('/Admin/ToggleUserStatus', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': token
        }
    })
    .then(response => {
        if (response.ok || response.redirected) {
            Swal.fire({
                title: 'Success!',
                text: `User has been ${isActive ? 'activated' : 'deactivated'} successfully!`,
                icon: 'success',
                confirmButtonColor: '#667eea',
                timer: 2000,
                timerProgressBar: true
            }).then(() => {
                // Reload the page to show updated status
                window.location.reload();
            });
            return;
        }
        return response.text();
    })
    .then(data => {
        if (data && typeof data === 'string') {
            // If we got HTML back, there might be an error
            throw new Error('Failed to update status');
        }
    })
    .catch(error => {
        console.error('Error:', error);
        Swal.fire({
            title: 'Error!',
            text: 'Failed to update user status. Please try again.',
            icon: 'error',
            confirmButtonColor: '#ff6b6b'
        });
    });
}

// Filter users
function filterUsers(filter) {
    const rows = document.querySelectorAll('#userTable tbody tr');
    rows.forEach(row => {
        if (filter === 'all') {
            row.style.display = '';
        } else if (filter === 'active') {
            row.style.display = row.querySelector('.badge.bg-success') ? '' : 'none';
        } else if (filter === 'inactive') {
            row.style.display = row.querySelector('.badge.bg-danger') ? '' : 'none';
        } else if (filter === 'admin') {
            row.style.display = row.querySelector('.badge.bg-warning') ? '' : 'none';
        }
    });
    
    // Update active filter button
    document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    event.target.classList.add('active');
}

// Export users to CSV
function exportUsersToCSV() {
    const table = document.getElementById('userTable');
    if (!table) return;
    
    exportTableToCSV('userTable', `MLM_Users_${new Date().toISOString().split('T')[0]}.csv`);
    showNotification('Users exported successfully!', 'success');
}

// Form enhancements
function initializeFormEnhancements() {
    // AJAX form submission for registration with SweetAlert
    const registerForm = document.getElementById('registerForm') || document.querySelector('form[action*="Register"]');
    if (registerForm && !registerForm.dataset.ajaxInitialized) {
        registerForm.dataset.ajaxInitialized = 'true';
        // Ensure form action is set correctly
        if (!registerForm.action || registerForm.action === '/' || registerForm.action.endsWith('/')) {
            registerForm.action = '/Account/Register';
        }
        registerForm.addEventListener('submit', function(e) {
            e.preventDefault(); // Always prevent default to use AJAX
            if (!this.checkValidity()) {
                this.classList.add('was-validated');
                return;
            }
            
            submitFormAjaxWithSweetAlert(this, {
                loadingTitle: 'Registering...',
                successTitle: 'Registration Successful!',
                successText: 'Your account has been created successfully! Redirecting to login...',
                redirectUrl: '/Account/Login'
            });
        });
    }
    
    // AJAX form submission for login with SweetAlert
    const loginForm = document.getElementById('loginForm') || document.querySelector('form[action*="Login"]');
    if (loginForm && !loginForm.dataset.ajaxInitialized) {
        loginForm.dataset.ajaxInitialized = 'true';
        // Ensure form action is set correctly
        if (!loginForm.action || loginForm.action === '/' || loginForm.action.endsWith('/')) {
            loginForm.action = '/Account/Login';
        }
        loginForm.addEventListener('submit', function(e) {
            e.preventDefault(); // Always prevent default to use AJAX
            if (!this.checkValidity()) {
                this.classList.add('was-validated');
                return;
            }
            
            submitFormAjaxWithSweetAlert(this, {
                loadingTitle: 'Logging in...',
                successTitle: 'Welcome Back!',
                successText: 'Login successful! Redirecting to dashboard...',
                redirectUrl: '/Dashboard'
            });
        });
    }
    
    // Sponsor ID validation
    const sponsorIdInput = document.querySelector('input[name="SponsorId"]');
    if (sponsorIdInput) {
        sponsorIdInput.addEventListener('blur', function() {
            validateSponsorId(this.value.trim());
        });
    }
    
    // Mobile number formatting
    const mobileInput = document.querySelector('input[name="MobileNumber"]');
    if (mobileInput) {
        mobileInput.addEventListener('input', function() {
            this.value = this.value.replace(/\D/g, '');
        });
    }
}

// AJAX form submission with SweetAlert2
function submitFormAjaxWithSweetAlert(form, options) {
    const formData = new FormData(form);
    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;
    
    // Disable button
    submitBtn.disabled = true;
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Processing...';
    
    // Get form action - handle empty or root action
    // form.action returns the full URL, so we need to extract just the path
    let formAction = form.action || form.getAttribute('action') || '';
    
    // Extract pathname from full URL if needed
    if (formAction && formAction.startsWith('http')) {
        try {
            const url = new URL(formAction);
            formAction = url.pathname + url.search;
        } catch (e) {
            // If URL parsing fails, try to extract path manually
            const match = formAction.match(/\/[^?#]*/);
            formAction = match ? match[0] : formAction;
        }
    }
    
    // Normalize: remove trailing slash if it's just root
    if (formAction === '/' || formAction === '' || formAction === window.location.pathname) {
        // Try to get action from form ID or data attributes
        if (form.id === 'loginForm') {
            formAction = '/Account/Login';
        } else if (form.id === 'registerForm') {
            formAction = '/Account/Register';
        } else if (form.action && (form.action.includes('Logout') || formAction.includes('Logout'))) {
            formAction = '/Account/Logout';
        } else {
            // Log for debugging
            console.error('Form action not properly set:', {
                formId: form.id,
                formAction: form.action,
                extractedAction: formAction,
                currentPath: window.location.pathname,
                form: form
            });
            Swal.fire({
                title: 'Error!',
                text: 'Form action is not properly configured. Please refresh the page.',
                icon: 'error',
                confirmButtonColor: '#ff6b6b'
            });
            submitBtn.disabled = false;
            submitBtn.innerHTML = originalText;
            return;
        }
    }
    
    // Ensure formAction starts with /
    if (!formAction.startsWith('/')) {
        formAction = '/' + formAction;
    }
    
    console.log('Submitting form to:', formAction);
    
    // Show loading SweetAlert
    Swal.fire({
        title: options.loadingTitle || 'Processing...',
        text: 'Please wait',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    fetch(formAction, {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': form.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        }
    })
    .then(response => {
        // Handle error responses
        if (!response.ok) {
            if (response.status === 400 || response.status === 500) {
                return response.json().then(err => {
                    throw new Error(err.error || `Server error: ${response.status}`);
                }).catch(() => {
                    return response.text().then(html => {
                        throw new Error(`Server error: ${response.status}`);
                    });
                });
            }
        }
        
        if (response.redirected) {
            Swal.fire({
                title: options.successTitle || 'Success!',
                text: options.successText || 'Operation completed successfully!',
                icon: 'success',
                confirmButtonColor: '#667eea',
                timer: 2000,
                timerProgressBar: true
            }).then(() => {
                window.location.href = response.url;
            });
        } else {
            return response.text();
        }
    })
    .then(html => {
        if (html) {
            // Parse response and show errors if any
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');
            const errors = doc.querySelectorAll('.text-danger, .validation-summary-errors, .alert-danger');
            
            if (errors.length > 0) {
                let errorMessages = [];
                errors.forEach(error => {
                    const text = error.textContent.trim();
                    if (text && !errorMessages.includes(text)) {
                        errorMessages.push(text);
                    }
                });
                
                Swal.fire({
                    title: 'Validation Error',
                    html: errorMessages.length > 0 ? errorMessages.join('<br>') : 'Please check your input',
                    icon: 'error',
                    confirmButtonColor: '#ff6b6b'
                });
            }
            
            submitBtn.disabled = false;
            submitBtn.innerHTML = originalText;
        }
    })
    .catch(error => {
        console.error('Error:', error);
        Swal.fire({
            title: 'Error!',
            text: 'An error occurred. Please try again.',
            icon: 'error',
            confirmButtonColor: '#ff6b6b'
        });
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalText;
    });
}

// Validate sponsor ID via AJAX
function validateSponsorId(sponsorId) {
    if (!sponsorId || sponsorId.length === 0) {
        return;
    }
    
    if (!sponsorId.match(/^REG\d+$/i)) {
        showFieldError('SponsorId', 'Sponsor ID must be in format REG followed by numbers (e.g., REG1001)');
        return;
    }
    
    // You can add AJAX validation here if needed
    // fetch(`/Account/ValidateSponsorId?sponsorId=${sponsorId}`)
    //     .then(response => response.json())
    //     .then(data => {
    //         if (!data.exists) {
    //             showFieldError('SponsorId', 'Sponsor ID does not exist');
    //         }
    //     });
}

// Show field error
function showFieldError(fieldName, message) {
    const field = document.querySelector(`[name="${fieldName}"]`);
    if (field) {
        field.classList.add('is-invalid');
        let errorSpan = field.parentElement.querySelector('.invalid-feedback');
        if (!errorSpan) {
            errorSpan = document.createElement('div');
            errorSpan.className = 'invalid-feedback';
            field.parentElement.appendChild(errorSpan);
        }
        errorSpan.textContent = message;
    }
}

// Real-time updates
function initializeRealTimeUpdates() {
    // Auto-refresh generation tree
    const treeContainer = document.getElementById('generationTree');
    if (treeContainer) {
        setInterval(() => {
            loadGenerationTree();
        }, 60000); // Refresh every minute
    }
}

// Load generation tree
function loadGenerationTree() {
    fetch('/Dashboard/GetGenerationTree')
        .then(response => response.json())
        .then(data => {
            if (data.userId) {
                renderTree(data, $('#generationTree'));
            }
        })
        .catch(error => {
            console.error('Error loading tree:', error);
        });
}

// Render generation tree
function renderTree(node, container) {
    if (!node || !node.userId) {
        container.html('<div class="alert alert-info">No generation tree data available.</div>');
        return;
    }

    let html = '<div class="tree-node-wrapper">';
    html += '<div class="tree-node ' + (node.isActive ? 'active' : 'inactive') + '">';
    html += '<div class="node-content">';
    html += '<div class="node-name"><strong>' + node.fullName + '</strong></div>';
    html += '<div class="node-id">' + node.userId + '</div>';
    if (node.email) {
        html += '<div class="node-email">' + node.email + '</div>';
    }
    html += '</div>';
    html += '</div>';

    if (node.children && node.children.length > 0) {
        html += '<div class="tree-children">';
        node.children.forEach(function(child) {
            html += '<div class="tree-child">';
            const childContainer = $('<div></div>');
            renderTree(child, childContainer);
            html += childContainer.html();
            html += '</div>';
        });
        html += '</div>';
    }

    html += '</div>';
    container.html(html);
}

// Show notification with SweetAlert2
function showNotification(message, type = 'info') {
    const icons = {
        'success': 'success',
        'danger': 'error',
        'error': 'error',
        'warning': 'warning',
        'info': 'info'
    };
    
    Swal.fire({
        title: type === 'success' ? 'Success!' : type === 'danger' ? 'Error!' : 'Info',
        text: message,
        icon: icons[type] || 'info',
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        customClass: {
            popup: 'animated-popup'
        }
    });
}

// Attach form handlers with SweetAlert2
function attachFormHandlers() {
    const toggleForms = document.querySelectorAll('form[action*="ToggleUserStatus"]');
    toggleForms.forEach(function(form) {
        // Remove old event listeners
        const newForm = form.cloneNode(true);
        form.parentNode.replaceChild(newForm, form);
        
        newForm.addEventListener('submit', function(e) {
            e.preventDefault();
            const userId = this.querySelector('input[name="userId"]').value;
            const isActive = this.querySelector('input[name="isActive"]').value === 'true';
            const action = isActive ? 'activate' : 'deactivate';
            const actionText = isActive ? 'activate' : 'deactivate';
            
            Swal.fire({
                title: `Are you sure?`,
                html: `Do you want to <strong>${actionText}</strong> this user?`,
                icon: actionText === 'activate' ? 'success' : 'warning',
                showCancelButton: true,
                confirmButtonColor: actionText === 'activate' ? '#4facfe' : '#fa709a',
                cancelButtonColor: '#6c757d',
                confirmButtonText: `Yes, ${actionText} it!`,
                cancelButtonText: 'Cancel',
                reverseButtons: true,
                customClass: {
                    popup: 'animated-popup'
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    toggleUserStatus(userId, isActive, newForm);
                }
            });
        });
    });
}

// Toggle user status via AJAX
function toggleUserStatus(userId, isActive, formElement) {
    const formData = new FormData();
    formData.append('userId', userId);
    formData.append('isActive', isActive);
    formData.append('__RequestVerificationToken', formElement.querySelector('input[name="__RequestVerificationToken"]').value);
    
    // Show loading
    Swal.fire({
        title: 'Processing...',
        text: 'Please wait',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    fetch('/Admin/ToggleUserStatus', {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': formElement.querySelector('input[name="__RequestVerificationToken"]').value
        }
    })
    .then(response => {
        if (response.redirected) {
            return response.url;
        }
        return response.text();
    })
    .then(data => {
        Swal.fire({
            title: 'Success!',
            text: `User has been ${isActive ? 'activated' : 'deactivated'} successfully!`,
            icon: 'success',
            confirmButtonColor: '#667eea',
            timer: 2000,
            timerProgressBar: true
        }).then(() => {
            // Reload the page to show updated status
            window.location.reload();
        });
    })
    .catch(error => {
        console.error('Error:', error);
        Swal.fire({
            title: 'Error!',
            text: 'Failed to update user status. Please try again.',
            icon: 'error',
            confirmButtonColor: '#ff6b6b'
        });
    });
}

// Get anti-forgery token
function getAntiForgeryToken() {
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    return tokenInput ? tokenInput.value : '';
}

// Copy to clipboard
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function() {
        Swal.fire({
            title: 'Copied!',
            text: 'Text copied to clipboard',
            icon: 'success',
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000
        });
    }).catch(function(err) {
        console.error('Failed to copy text: ', err);
        Swal.fire({
            title: 'Error!',
            text: 'Failed to copy text',
            icon: 'error',
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000
        });
    });
}

// Export table to CSV
function exportTableToCSV(tableId, filename) {
    const table = document.getElementById(tableId);
    if (!table) return;
    
    let csv = [];
    const rows = table.querySelectorAll('tr');
    
    for (let i = 0; i < rows.length; i++) {
        const row = [], cols = rows[i].querySelectorAll('td, th');
        
        for (let j = 0; j < cols.length; j++) {
            let data = cols[j].innerText.replace(/(\r\n|\n|\r)/gm, '').replace(/"/g, '""');
            row.push('"' + data + '"');
        }
        
        csv.push(row.join(','));
    }
    
    const csvFile = new Blob([csv.join('\n')], { type: 'text/csv' });
    const downloadLink = document.createElement('a');
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = 'none';
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}

// Smooth scroll
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});
