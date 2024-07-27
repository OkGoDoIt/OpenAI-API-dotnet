#!/bin/bash
# Default values
DEFAULT
_PATH="/home/runner/work/OpenAI-API-dotnet/OpenAI-API-dotnet/package.json"
FALLBACK
_DIR="/home/runner/work/OpenAI-API-dotnet/OpenAI-API-dotnet/"
LOG
FILE="install
_
_log.txt"
# Function to log messages
log_message() {
local message="$1"
echo "$(date '+%Y-%m-%d %H:%M:%S') - $message" >> "$LOG
FILE"
_
}
# Function to check if a file exists
file
_exists() {
[ -f "$1" ]
}
# Function to find package.json in a directory
find
_package_json() {
local search
dir="$1"
_
find "$search
_dir" -name "package.json" -print -quit
}
# Main script
log_message "Starting path correction and installation process..."
# Check if package.json exists at the default path
if file
exists "$DEFAULT
_
_PATH"; then
log_message "package.json found at $DEFAULT
_
PATH."
else
log_message "package.json not found at $DEFAULT
_
PATH."
# Attempt to locate package.json in the fallback directory
log_message "Searching for package.json in fallback directory $FALLBACK
_
PACKAGE
JSON
_
_
PATH=$(find_package_json "$FALLBACK
_DIR")
DIR..."
if [ -n "$PACKAGE
JSON
_
_PATH" ]; then
log_message "package.json found at $PACKAGE
JSON
_
_
DEFAULT
PATH="$PACKAGE
JSON
PATH"
_
_
_
PATH."
else
log_message "Error: package.json not found in fallback directory."
echo "Error: package.json not found. Check the log file $LOG
_
exit 1
FILE for details."
fi
fi
# Navigate to the directory containing package.json
WORKING
_
DIR=$(dirname "$DEFAULT
_PATH")
log_message "Changing directory to $WORKING
DIR"
_
cd "$WORKING
_DIR" || { log_message "Failed to change directory to $WORKING
_DIR."; echo
"Failed to change directory. Check the log file $LOG
_FILE for details."; exit 1; }
# Run npm install
log_message "Running npm install..."
npm install >> "$LOG
FILE" 2>&1
_
# Check if npm install was successful
if [ $? -eq 0 ]; then
log_message "npm install completed successfully."
else
log_message "Error: npm install failed."
echo "Error: npm install failed. Check the log file $LOG
_
exit 1
FILE for details."
fi
