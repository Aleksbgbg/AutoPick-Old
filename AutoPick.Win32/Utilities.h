#pragma once

#include "Win.h"

HWND AutoPick_FindWindow(LPCSTR windowName);

LONG ConvertCoordinate(const int coordinate, const int metricIndex);

INPUT MouseClickInput(POINT targetClickPosition);

void FillKeyPressInput(INPUT* inputs, const WORD virtualKey, const WORD scan, const DWORD flags);