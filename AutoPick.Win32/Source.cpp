#define EXTERN_DECL extern "C" __declspec(dllexport)
#define CALL_CONV __cdecl

#include <locale>
#include <codecvt>
#include <string>

#include "Utilities.h"

EXTERN_DECL bool CALL_CONV IsWindowOpen(LPCSTR windowName)
{
	return AutoPick_FindWindow(windowName) != nullptr;
}

EXTERN_DECL bool CALL_CONV IsWindowMinimised(LPCSTR windowName)
{
	return IsWindowVisible(AutoPick_FindWindow(windowName)) == 0;
}

EXTERN_DECL HBITMAP CALL_CONV CaptureWindow(LPCSTR windowName)
{
	HWND hwnd = AutoPick_FindWindow(windowName);

	HDC sourceDeviceContext = GetDC(hwnd);
	HDC targetDeviceContext = CreateCompatibleDC(sourceDeviceContext);

	RECT windowRect{ };
	GetWindowRect(hwnd, &windowRect);

	HBITMAP bitmap = CreateCompatibleBitmap
	(
		sourceDeviceContext,
		windowRect.right - windowRect.left,
		windowRect.bottom - windowRect.top
	);
	SelectObject(targetDeviceContext, bitmap);

	PrintWindow(hwnd, targetDeviceContext, PW_CLIENTONLY);

	ReleaseDC(hwnd, sourceDeviceContext);
	DeleteDC(targetDeviceContext);

	return bitmap;
}

EXTERN_DECL void CALL_CONV CleanUpWindowCapture(HBITMAP bitmap)
{
	DeleteObject(bitmap);
}

EXTERN_DECL void CALL_CONV Click(LPCSTR windowName, const int x, const int y)
{
	POINT targetClickPosition;
	targetClickPosition.x = x;
	targetClickPosition.y = y;
	
	HWND hwnd = AutoPick_FindWindow(windowName);
	ClientToScreen(hwnd, &targetClickPosition);

	INPUT input = MouseClickInput(targetClickPosition);

	SetForegroundWindow(hwnd);
	SendInput(1, &input, sizeof(INPUT));
}

EXTERN_DECL void CALL_CONV TypeAt(LPCSTR windowName, const int x, const int y, LPCSTR text)
{
	const int textLength = strlen(text);
	const int inputCount = (textLength * 2) + 1;
	INPUT* inputs = new INPUT[inputCount];

	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>> converter{ };
	std::wstring wideText = converter.from_bytes(text);

	for (int charIndex = 0; charIndex < textLength; ++charIndex)
	{
		wchar_t character = wideText[charIndex];
		const int inputIndex = (charIndex * 2) + 1;

		FillKeyPressInput(inputs + inputIndex, 0, character, KEYEVENTF_UNICODE);
	}

	POINT targetTypePosition;
	targetTypePosition.x = x;
	targetTypePosition.y = y;

	HWND hwnd = AutoPick_FindWindow(windowName);

	ClientToScreen(hwnd, &targetTypePosition);
	inputs[0] = MouseClickInput(targetTypePosition);

	SetForegroundWindow(hwnd);
	SendInput(inputCount, inputs, sizeof(INPUT));

	delete[] inputs;
}

EXTERN_DECL void CALL_CONV PressEnter(LPCSTR windowName)
{
	static constexpr int InputCount = 2;
	INPUT inputs[InputCount];

	FillKeyPressInput(inputs, VK_RETURN, 0, 0);

	HWND hwnd = AutoPick_FindWindow(windowName);
	SetForegroundWindow(hwnd);
	SendInput(InputCount, inputs, sizeof(INPUT));
}