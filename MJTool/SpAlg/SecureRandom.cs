﻿/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013-4-16
 * 时间: 18:38
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace MJTool
{
	/// <summary>
	/// Random number generator - requires a PRNG backend, e.g. prng4.js
	/// For best results, put code like
	/// <body onClick='rng_seed_time();' onKeyPress='rng_seed_time();'>
	/// in your main HTML document.
	/// </summary>
	public class SecureRandom
	{
		private Arcfour rng_state;
		private int[] rng_pool;
		private int rng_pptr;
		
		// Pool size must be a multiple of 4 and greater than 32.
		// An array of bytes the size of the pool will be passed to init()
		public static int rng_psize = 256;
		public SecureRandom()
		{
		}
		
		// Mix in a 32-bit integer into the pool
		private void rng_seed_int(long x)
		{
			rng_pool[rng_pptr++] ^= Convert.ToInt32(x & (long)255);
			rng_pool[rng_pptr++] ^= Convert.ToInt32((x >> 8) & (long)255);
			rng_pool[rng_pptr++] ^= Convert.ToInt32((x >> 16) & (long)255);
			rng_pool[rng_pptr++] ^= Convert.ToInt32((x >> 24) & (long)255);
			if (rng_pptr >= rng_psize)
			{
				rng_pptr -= rng_psize;
			}
		}
		
		// Mix in the current time (w/milliseconds) into the pool
		private void rng_seed_time()
		{
			rng_seed_int(QueryManager.UnixTimeStamp(DateTime.Now));
		}
		
		public static void init_pool()
		{
			if (rng_pool == null)
			{
				rng_pool = new int[rng_psize];
				rng_pptr = 0;
				int t;
				/*
				if (navigator.appName == "Netscape" && navigator.appVersion < "5"
				    && window.crypto
				    && typeof (window.crypto.random) === 'function') {
					// Extract entropy (256 bits) from NS4 RNG if available
					var z = window.crypto.random(32);
					for (t = 0; t < z.length; ++t)
						rng_pool[rng_pptr++] = z.charCodeAt(t) & 255;
				}
				 */
				while (rng_pptr < rng_psize)
				{
					// extract some randomness from
					// Math.random()
					Random r = new Random();
					t = Convert.ToInt32(Math.Floor(r.NextDouble() * 65536));
					rng_pool[rng_pptr++] = t >> 8;
					rng_pool[rng_pptr++] = t & 255;
				}
				rng_pptr = 0;
				rng_seed_time();
				// rng_seed_int(window.screenX);
				// rng_seed_int(window.screenY);
			}
		}
		
		private int rng_get_byte()
		{
			if (rng_state == null)
			{
				rng_seed_time();
				rng_state = new Arcfour();
				rng_state.init(rng_pool);
				for (rng_pptr = 0; rng_pptr < rng_pool.Length; ++rng_pptr)
				{
					rng_pool[rng_pptr] = 0;
				}
				rng_pptr = 0;
				// rng_pool = null;
			}
			// TODO: allow reseeding after first request
			return rng_state.next();
		}

		public void nextBytes(int[] ba)
		{
			int i;
			for (i = 0; i < ba.Length; ++i)
			{
				ba[i] = rng_get_byte();
			}
		}
	}
}
