#include "Utilities.h"

HWND AutoPick_FindWindow(LPCSTR windowName)
{
	return FindWindow(nullptr, windowName);
}

LONG ConvertCoordinate(const int coordinate, const int metricIndex)
{
	return (coordinate * 65536) / GetSystemMetrics(metricIndex);
}

INPUT MouseClickInput(POINT targetClickPosition)
{
	INPUT input{ };
	input.type = INPUT_MOUSE;
	input.mi.dx = ConvertCoordinate(targetClickPosition.x, SM_CXSCREEN);
	input.mi.dy = ConvertCoordinate(targetClickPosition.y, SM_CYSCREEN);
	input.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;

	return input;
}

void FillKeyPressInput(INPUT* inputs, const WORD virtualKey, const WORD scan, const DWORD flags)
{
	INPUT input{ };
	input.type = INPUT_KEYBOARD;
	input.ki.wVk = virtualKey;
	input.ki.wScan = scan;
	input.ki.dwFlags = flags;

	inputs[0] = input;
	inputs[1] = input;

	inputs[1].ki.dwFlags |= KEYEVENTF_KEYUP;
}