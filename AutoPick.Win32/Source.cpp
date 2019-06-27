#define EXTERN_DECL extern "C" __declspec(dllexport)
#define CALL_CONV __cdecl

#include <Windows.h>

HWND AutoPick_FindWindow(LPCSTR windowName)
{
	return FindWindow(nullptr, windowName);
}

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