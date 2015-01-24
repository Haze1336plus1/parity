/*
 * WCRT  -  Win32API CRT
 *
 * ctype.h
 *
 * Copyright (c) 2003-2004 by Joergen Ibsen / Jibz
 * All Rights Reserved
 *
 * http://www.ibsensoftware.com/
 *
 * This software is provided 'as-is', without any express
 * or implied warranty.  In no event will the authors be
 * held liable for any damages arising from the use of
 * this software.
 *
 * Permission is granted to anyone to use this software
 * for any purpose, including commercial applications,
 * and to alter it and redistribute it freely, subject to
 * the following restrictions:
 *
 * 1. The origin of this software must not be
 *    misrepresented; you must not claim that you
 *    wrote the original software. If you use this
 *    software in a product, an acknowledgment in
 *    the product documentation would be appreciated
 *    but is not required.
 *
 * 2. Altered source versions must be plainly marked
 *    as such, and must not be misrepresented as
 *    being the original software.
 *
 * 3. This notice may not be removed or altered from
 *    any source distribution.
 */

#ifndef WCRT_CTYPE_H_INCLUDED
#define WCRT_CTYPE_H_INCLUDED

#include <stddef.h>

#ifdef __cplusplus
extern "C" {
#endif

    int isalnum(int c);
    int isalpha(int c);
    int iscntrl(int c);
    int ispunct(int c);
    int isspace(int c);
    int isxdigit(int c);

    #define isdigit(c) (((c) - '0') < 10U)
    #define isgraph(c) (((c) - '!') < 94U)
    #define islower(c) (((c) - 'a') < 26U)
    #define isprint(c) (((c) - ' ') < 95U)
    #define isupper(c) (((c) - 'A') < 26U)

    int tolower(int c);
    int toupper(int c);

#ifdef __cplusplus
} /* extern "C" */
#endif

#endif /* WCRT_CTYPE_H_INCLUDED */
