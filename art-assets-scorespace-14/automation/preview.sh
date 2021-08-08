#!/bin/sh

cd Assets/Art/automation/  # intended to be called from nested root
git fetch origin
git diff --stat main origin/main
