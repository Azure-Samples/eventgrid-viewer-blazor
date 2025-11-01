#!/bin/bash

################################################################################################
#- Purpose: Script to set secure permissions on log files and directories
#- This script should be run after deployment to ensure log files have restricted access
################################################################################################

set -eu

# Create logs directory if it doesn't exist
LOG_DIR="logs"
if [ ! -d "$LOG_DIR" ]; then
    echo "Creating logs directory..."
    mkdir -p "$LOG_DIR"
fi

# Set secure permissions on logs directory
echo "Setting secure permissions on logs directory..."
chmod 750 "$LOG_DIR"

# Set secure permissions on existing log files
if [ -n "$(find "$LOG_DIR" -name "*.txt" 2>/dev/null)" ]; then
    echo "Setting secure permissions on existing log files..."
    find "$LOG_DIR" -name "*.txt" -exec chmod 640 {} \;
else
    echo "No existing log files found."
fi

# Set ownership if running as root (for production deployment)
if [ "$EUID" -eq 0 ]; then
    echo "Setting ownership for production deployment..."
    # Adjust these user/group names based on your deployment
    chown -R www-data:www-data "$LOG_DIR" 2>/dev/null || echo "Warning: Could not set www-data ownership"
fi

echo "Log file security setup completed!"
echo "Directory permissions: $(ls -ld "$LOG_DIR")"

if [ -n "$(find "$LOG_DIR" -name "*.txt" 2>/dev/null)" ]; then
    echo "File permissions:"
    find "$LOG_DIR" -name "*.txt" -exec ls -l {} \;
fi