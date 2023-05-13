import numpy as np
import einops
from PIL import Image

SIZE  = 256
RATIO = 1
COLOR = [128, 0, 0, 255]

radius = SIZE / 2 * RATIO

coords = einops.rearrange(np.meshgrid(np.linspace(0, 1 / RATIO, SIZE), np.linspace(0, 1 / RATIO, SIZE), indexing='ij'), 'c h w -> h w c')
mask = ((coords - (np.array([0.5, 0.5]) / RATIO) [None, None]) ** 2).sum(axis=-1) < 0.25

img = einops.repeat(np.array(COLOR), 'd -> h w d', h=SIZE, w=SIZE)
img = img * mask[..., None]

img = Image.fromarray(img.astype(np.uint8))

img.save('output.png')